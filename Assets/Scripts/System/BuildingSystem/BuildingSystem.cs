using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : UIRequirementDisplay
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
    private bool canCreateBuilding = false;

    [Header("Architecture Prefabs")]
    private GameObject architectureSelected;
    private BuildingData currentBuildingRequirement;

    [Header("Actions")]
    public static Action generateButton;

    private List<GameObject> placedBuildings = new List<GameObject>(); //  Thêm danh sách công trình đã đặt

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
            generateButton?.Invoke();
        }
    }

    public void SelectArchitecture(BuildingData buildingData)
    {
        currentBuildingRequirement = buildingData;

        if (architectureSelected != null || !playerToolSelector.CanBuild())
        {
            Destroy(architectureSelected);
        }

        architectureSelected = Instantiate(buildingData.buildingPrefab, checkTransform.position, Quaternion.identity);
        ToggleBuildingMode(architectureSelected);

        ShowRequiredItems(
            buildingData.requiredItems,
            buildingData.requiredAmounts,
            DataManagers.instance.GetItemSpriteFromName
        );

        Debug.Log($" Đã chọn {buildingData.buildingName}");
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

        Vector3 targetPosition = checkTransform.position + checkTransform.forward * 7f;
        architecture.transform.position = new Vector3(targetPosition.x, 11.5f, targetPosition.z);
    }

    private void ValidatePlacement()
    {
        if (architectureSelected == null) return;

        isOnGround = false;
        hasObstacle = false;

        MeshRenderer[] renderers = architectureSelected.GetComponentsInChildren<MeshRenderer>();
        if (renderers.Length == 0) return;

        Color targetColor = Color.green;

        Bounds bounds = renderers[0].bounds;
        Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity);

        foreach (Collider col in colliders)
        {
            if (col.gameObject == architectureSelected || col.transform.IsChildOf(architectureSelected.transform))
                continue;

            if (col.CompareTag("Ground") || col.CompareTag("FarmArea"))
            {
                isOnGround = true;
            }
            else
            {
                hasObstacle = true;
                targetColor = Color.red;
            }
        }

        foreach (MeshRenderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                mat.color = targetColor;
            }
        }
    }

    public void PlaceBuilding()
    {
        if (architectureSelected == null || hasObstacle) return;
        if (currentBuildingRequirement == null) return;

        bool hasEnough = true; //  Mặc định có đủ nguyên liệu

        //  Kiểm tra đủ nguyên liệu hay không
        for (int i = 0; i < currentBuildingRequirement.requiredItems.Length; i++)
        {
            string itemName = currentBuildingRequirement.requiredItems[i];
            int requiredAmount = currentBuildingRequirement.requiredAmounts[i];

            if (!inventoryManager.FindItemByName(itemName, requiredAmount))
            {
                hasEnough = false;
                break; //  Nếu thiếu nguyên liệu, dừng kiểm tra ngay
            }
        }

        if (!hasEnough)
        {
            Debug.LogWarning("Không đủ nguyên liệu để xây dựng!");
            return;
        }

        //  Trừ nguyên liệu trong InventoryManager
        for (int i = 0; i < currentBuildingRequirement.requiredItems.Length; i++)
        {
            inventoryManager.GetInventory().RemoveItemByName(
                currentBuildingRequirement.requiredItems[i],
                currentBuildingRequirement.requiredAmounts[i]
            );
        }

        Debug.Log("Đủ nguyên liệu! Tiến hành xây dựng...");

        //  Đặt công trình vào thế giới
        GameObject placedObject = Instantiate(
            currentBuildingRequirement.buildingPrefab,
            architectureSelected.transform.position,
            architectureSelected.transform.rotation
        );

        //  Gán `BuildingData` vào component `PlacedBuilding`
        PlacedBuilding placedComponent = placedObject.AddComponent<PlacedBuilding>();
        placedComponent.buildingData = currentBuildingRequirement;

        placedBuildings.Add(placedObject); // Thêm vào danh sách công trình đã đặt

        //  Gửi sự kiện xây dựng hoàn tất
        PlacedBuildingData placedData = new PlacedBuildingData(
            currentBuildingRequirement.buildingName,
            placedObject.transform.position,
            placedObject.transform.rotation
        );
        EventBus.Publish(new BuildingPlacedEvent(placedData));

        ResetBuildingColor();

        //  Xóa mô hình preview
        Destroy(architectureSelected);
        architectureSelected = null;

        Debug.Log("Công trình đã được xây dựng thành công!");
        inventoryManager.GetInventoryDisplay().UpdateDisplay(inventoryManager.GetInventory());
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

    public void LoadBuilding(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return;

        GameObject placedObject = Instantiate(prefab, position, rotation);
        placedBuildings.Add(placedObject); //  Thêm vào danh sách công trình đã đặt

        Debug.Log($" Đã tải công trình '{prefab.name}' tại {position}!");
    }

    public List<GameObject> GetPlacedBuildings()
    {
        return placedBuildings;
    }

    public void ClearPlacedBuildings()
    {
        foreach (var building in GetPlacedBuildings())
        {
            Destroy(building.gameObject);
        }

        // Xóa danh sách các công trình đã đặt
        placedBuildings.Clear();
        Debug.Log("🔴 Đã xóa toàn bộ công trình cũ trước khi load!");
    }

}
