using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSystem : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform checkTransform;
    [SerializeField] private LayerMask groundLayer;
    private PlayerToolSelector playerToolSelector;
    [SerializeField] private GameObject BuildSystemUI;
    [SerializeField] private InventoryManager inventoryManager;

    [Header("Settings")]
    private bool isBuildingMode = false;
    private bool isPositionValid = false;
    [SerializeField] private bool hasObstacle = false;
    private bool isOnGround = false;
    private bool notEnoughItem = false;

    [Header("Architecture Prefabs")]
    private GameObject architectureSelected;
    private BuildingData currentBuildingRequirement;


    [Header("UI Elements")]
    [SerializeField] private Transform requirementContainer; // Chứa danh sách nguyên liệu
    [SerializeField] private GameObject requiredItemPrefab; // Prefab hiển thị nguyên liệu

    [Header("Actions")]
    public static Action generateButton;
    
    private void Start()
    {
        UISelectButton.buildButtonPressed += SelectArchitecture;
        architectureSelected = null;
        playerToolSelector = GetComponent<PlayerToolSelector>();
        playerToolSelector.onToolSelected += ToolSelectedCallBack;
        BuildSystemUI.SetActive(false);

        ActionButton.Building += PlaceBuilding;
    }

    private void OnDestroy()
    {
        playerToolSelector.onToolSelected -= ToolSelectedCallBack;
        ActionButton.Building -= PlaceBuilding;
        UISelectButton.buildButtonPressed -= SelectArchitecture;

    }

    private void Update()
    {
        if (isBuildingMode && architectureSelected != null)
        {
            HandleBuildingMovement(architectureSelected);
            ValidatePlacement();
        }
    }

    private void ToolSelectedCallBack(PlayerToolSelector.Tool selectedTool)
    {
        BuildSystemUI.SetActive(playerToolSelector.CanBuild());

        if (selectedTool == PlayerToolSelector.Tool.Hammer)
        {
            generateButton?.Invoke();//UISelectButton
        }
    }

    


    public void SelectArchitecture(BuildingData buildingData)
    {
        currentBuildingRequirement = buildingData;

        if (architectureSelected != null || !playerToolSelector.CanBuild())
        {
            Destroy(architectureSelected);
        }

        // Hiển thị mô hình preview
        architectureSelected = Instantiate(buildingData.buildingPrefab);
        ToggleBuildingMode(architectureSelected);

        // Hiển thị nguyên liệu
        ShowRequiredItems(buildingData);

        Debug.Log($"✅ Đã chọn {buildingData.buildingName}");

        
    }




    private void ShowRequiredItems(BuildingData buildingData)
    {
        // Xóa danh sách cũ
        foreach (Transform child in requirementContainer)
        {
            Destroy(child.gameObject);
        }

        // Kiểm tra nếu buildingData có nguyên liệu yêu cầu
        if (buildingData.requiredItems == null || buildingData.requiredItems.Length == 0)
        {
            requirementContainer.gameObject.SetActive(false);
            return;
        }

        requirementContainer.gameObject.SetActive(true);

        // Lặp qua danh sách nguyên liệu
        for (int i = 0; i < buildingData.requiredItems.Length; i++)
        {
            GameObject itemUI = Instantiate(requiredItemPrefab, requirementContainer);

            // Gán dữ liệu vào UI
            Image iconImage = itemUI.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI nameText = itemUI.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI amountText = itemUI.transform.Find("Amount").GetComponent<TextMeshProUGUI>();

            // Lấy icon từ DataManagers (hoặc từ BuildingData nếu có)
            Sprite itemIcon = DataManagers.instance.GetItemSpriteFromName(buildingData.requiredItems[i]);
            if (itemIcon != null)
            {
                iconImage.sprite = itemIcon;
            }
            else
            {
                iconImage.sprite = buildingData.requiredItemsIcon; // Dùng icon mặc định nếu không có
            }

            nameText.text = buildingData.requiredItems[i];
            amountText.text = $"x{buildingData.requiredAmounts[i]}";


        }
    }

    private void ToggleBuildingMode(GameObject architecture)
    {
        if (!isBuildingMode)
        {
            if (architectureSelected != null)
            {
                Destroy(architectureSelected);
            }
            architectureSelected = Instantiate(architecture);
            architectureSelected.SetActive(true);
            isBuildingMode = true;
        }
        else
        {
            if (architectureSelected != null)
            {
                Destroy(architectureSelected);
                architectureSelected = null;
            }
            isBuildingMode = false;
        }
    }

    private void HandleBuildingMovement(GameObject architecture)
    {
        if (architecture == null) return;

        float distanceFromPlayer = 7f;
        Vector3 targetPosition = checkTransform.position + (checkTransform.forward * distanceFromPlayer);

        if (Physics.Raycast(targetPosition + Vector3.up * 5, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            architecture.transform.position = new Vector3(hit.point.x, hit.point.y + 2f, hit.point.z);
        }
    }

    private void ValidatePlacement()
    {
        if (architectureSelected == null) return;

        isOnGround = false;
        hasObstacle = false;

        // Lấy tất cả MeshRenderer của công trình để đổi màu
        MeshRenderer[] renderers = architectureSelected.GetComponentsInChildren<MeshRenderer>();
        if (renderers.Length == 0) return; // Nếu không có renderer, thoát sớm

        // Mặc định đặt màu xanh lá cây (có thể thay đổi nếu phát hiện vật cản)
        Color targetColor = Color.green;

        // Lấy danh sách các collider xung quanh công trình
        Bounds bounds = renderers[0].bounds;
        Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity);

        foreach (Collider col in colliders)
        {
            if (col.gameObject == architectureSelected || col.transform.IsChildOf(architectureSelected.transform))
                continue;

            if (col.CompareTag("Ground"))
            {
                isOnGround = true;
                Debug.Log("isground");
            }
            else
            {
                hasObstacle = true;
                Debug.Log("hasObstacle");

                // Nếu phát hiện vật thể cản trở -> đổi màu sang đỏ
                targetColor = Color.red;
            }
        }

        // Đổi màu toàn bộ kiến trúc dựa trên kết quả kiểm tra
        foreach (MeshRenderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                mat.color = targetColor;
            }
        }
    }



    private void PlaceBuilding()
    {
        if (architectureSelected == null || hasObstacle) return;
        if (currentBuildingRequirement == null) return;

        bool hasEnough = true; // ✅ Mặc định là true, sẽ chuyển thành false nếu thiếu nguyên liệu

        // Kiểm tra từng nguyên liệu
        for (int i = 0; i < currentBuildingRequirement.requiredItems.Length; i++)
        {
            string itemName = currentBuildingRequirement.requiredItems[i];  // Lấy từng item
            int requiredAmount = currentBuildingRequirement.requiredAmounts[i]; // Số lượng cần

            // Nếu thiếu bất kỳ nguyên liệu nào => Không đủ, dừng kiểm tra
            if (!inventoryManager.FindItemByName(itemName, requiredAmount))
            {
                hasEnough = false;
                break; // ❌ Nếu thiếu nguyên liệu, dừng kiểm tra ngay
            }
        }

        Debug.Log("Trạng thái nguyên liệu: " + hasEnough);

        // Nếu không đủ nguyên liệu, không xây dựng
        if (!hasEnough)
        {
            Debug.LogWarning("❌ Không đủ nguyên liệu để xây dựng!");
            return;
        }

        // ✅ Đủ nguyên liệu -> Trừ nguyên liệu & Tiến hành xây dựng
        for (int i = 0; i < currentBuildingRequirement.requiredItems.Length; i++)
        {
            inventoryManager.GetInventory().RemoveItemByName(currentBuildingRequirement.requiredItems[i], currentBuildingRequirement.requiredAmounts[i]);
        }

        Debug.Log("✅ Nguyên liệu hợp lệ! Tiến hành xây dựng...");

        // Đặt công trình
        Instantiate(currentBuildingRequirement.buildingPrefab, architectureSelected.transform.position, architectureSelected.transform.rotation);

        ResetBuildingColor();

        // Xóa preview
        Destroy(architectureSelected);
        architectureSelected = null;

        Debug.Log("🏗️ Công trình đã được xây dựng thành công!");
    }





    private void ResetBuildingColor()
    {
        if (architectureSelected == null) return;

        MeshRenderer[] renderers = architectureSelected.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.color = Color.white;
        }
    }

    public void RotateBuilding()
    {
        if (isBuildingMode && architectureSelected != null)
        {
            architectureSelected.transform.rotation *= Quaternion.Euler(0, 90, 0);
        }
    }






}
