using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSystem : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] private Transform checkTransform;
    [SerializeField] private LayerMask groundLayer;
    private PlayerToolSelector playerToolSelector;
    [SerializeField] GameObject BuildSystemUI;


    [Header("Settings")]
    private bool isBuildingMode = false;
    private bool isPositionValid = false;
    [SerializeField] private bool hasObstacle;
    private bool isOnGround;
    [Header("Architecture")]
    private GameObject architectureSelected;
    [Header("Architecture Prefab")]
    [SerializeField] GameObject ChickenCoop;
    [SerializeField] GameObject Fence;


    private void Start()
    {
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
        if (!playerToolSelector.CanBuild())
        {
            BuildSystemUI.SetActive(false);
        }
        else
        {
            BuildSystemUI.SetActive(true);
        }
    }

    public void Selectarchitecture(Button button)
    {
        if (architectureSelected != null)
        {
            Destroy(architectureSelected);
        }

        string buttonName = button.gameObject.name;
        if (buttonName == "ChickenCoop")
        {
            architectureSelected = Instantiate(ChickenCoop);
            ToggleBuildingMode(architectureSelected);
            Debug.Log("SELECTED ChickenCoop");
        }
        else if (buttonName == "Fence")
        {
            architectureSelected = Instantiate(Fence);
            ToggleBuildingMode(architectureSelected);
            Debug.Log("SELECTED Fence");
        }
    }


    private void ToggleBuildingMode(GameObject architecture)
    {
        if (!isBuildingMode) // Nếu đang tắt thì bật lên
        {
            if (architectureSelected != null)
            {
                Destroy(architectureSelected);
            }
            architectureSelected = Instantiate(architecture);
            architectureSelected.SetActive(true);
            isBuildingMode = true;
        }
        else // Nếu đang bật thì tắt
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

        float distanceFromPlayer = 5f;
        Vector3 targetPosition = checkTransform.position + (checkTransform.forward * distanceFromPlayer);

        RaycastHit hit;
        if (Physics.Raycast(targetPosition + Vector3.up * 5, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            architecture.transform.position = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);
        }
    }

    private void ValidatePlacement()
    {
        if (architectureSelected == null) return;

        // Reset trạng thái
        isOnGround = false;
        hasObstacle = false;

        // Kiểm tra va chạm bằng OverlapBox
        Bounds bounds = architectureSelected.GetComponentInChildren<Renderer>().bounds;
        Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity);

        foreach (Collider col in colliders)
        {
            if (col.gameObject == architectureSelected || col.transform.IsChildOf(architectureSelected.transform))
                continue;

            Debug.Log($"Va chạm với: {col.gameObject.name}, Tag: {col.tag}, Layer: {LayerMask.LayerToName(col.gameObject.layer)}");

            if (col.CompareTag("Ground"))
            {
                isOnGround = true;
            }
            else
            {
                hasObstacle = true;
            }
        }

        // Xác định màu sắc
        isPositionValid = isOnGround && !hasObstacle;
        Color targetColor = isPositionValid ? Color.green : Color.red;

        // Cập nhật màu sắc
        MeshRenderer[] renderers = architectureSelected.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.material.color = targetColor;
            }
        }

        Debug.Log(isPositionValid ? "Có thể build" : "Không thể build");
    }


    private void PlaceBuilding()
    {
        if (architectureSelected == null || hasObstacle) return;

        // Tạo tòa nhà thực sự
        Instantiate(architectureSelected, architectureSelected.transform.position, architectureSelected.transform.rotation);

        // Reset màu về mặc định (trắng)
        ResetBuildingColor();

        // Hủy object preview
        Destroy(architectureSelected);
        architectureSelected = null;
    }

    private void ResetBuildingColor()
    {
        if (architectureSelected == null) return;

        MeshRenderer[] renderers = architectureSelected.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.material.color = Color.white; // Đặt về màu trắng mặc định
            }
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
