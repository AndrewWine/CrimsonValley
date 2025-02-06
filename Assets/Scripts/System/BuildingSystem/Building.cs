using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject buildingPrefab;  // Prefab của công trình
    [SerializeField] private Transform checkTransform;  // Đối tượng để kiểm tra vị trí xây dựng

    [Header("Settings")]
    private GameObject currentBuilding;  // Công trình hiện tại đang được di chuyển
    private bool isBuildingMode = false;  // Kiểm tra chế độ xây dựng
    private bool isPositionValid = true;  // Kiểm tra xem vị trí có hợp lệ không

    [SerializeField] private LayerMask groundLayer;  // Layer mặt đất để kiểm tra va chạm

    private void Start()
    {
        if (buildingPrefab == null)
        {
            Debug.LogError("Building prefab chưa được gán trong Inspector!");
        }

        if (checkTransform == null)
        {
            Debug.LogError("CheckTransform chưa được gán!");
        }
    }

    private void Update()
    {
        // Bật chế độ xây dựng khi nhấn phím "B"
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildingMode();
        }

        // Nếu chế độ xây dựng bật, thực hiện các hành động
        if (isBuildingMode)
        {
            HandleBuildingMovement();
            HandleBuildingPlacement();
        }
    }

    private void ToggleBuildingMode()
    {
        isBuildingMode = !isBuildingMode;

        // Khi chế độ xây dựng bật, tạo ra một công trình để di chuyển
        if (isBuildingMode)
        {
            currentBuilding = Instantiate(buildingPrefab);
            currentBuilding.SetActive(true);  // Hiển thị vật thể xây dựng
        }
        else
        {
            Destroy(currentBuilding);  // Xóa vật thể nếu chế độ xây dựng tắt
        }
    }

    private void HandleBuildingMovement()
    {
        if (currentBuilding == null) return;

        // Xác định khoảng cách cố định giữa người chơi và building
        float distanceFromPlayer = 5f;  // Khoảng cách từ người chơi đến building

        // Lấy vị trí người chơi
        Vector3 playerPosition = checkTransform.position;

        // Lấy vị trí raycast và di chuyển building cách xa người chơi theo hướng nhìn của người chơi
        RaycastHit hit;
        Vector3 startRayPosition = playerPosition + Vector3.up * 5;  // Bắt đầu từ vị trí người chơi, cao hơn 5 đơn vị
        Vector3 rayDirection = Vector3.down;

        if (Physics.Raycast(startRayPosition, rayDirection, out hit, Mathf.Infinity, groundLayer))
        {
            // Đặt vị trí building cách người chơi một khoảng cách nhất định trên trục X và Z
            Vector3 targetPosition = hit.point + (checkTransform.forward * distanceFromPlayer);

            // Nâng vị trí Y của building lên 5 đơn vị để không đè lên mặt đất
            currentBuilding.transform.position = new Vector3(targetPosition.x, hit.point.y + 0.5f, targetPosition.z);

            isPositionValid = true;
        }
        else
        {
            isPositionValid = false;
        }

        // Kiểm tra và thay đổi màu sắc của các MeshRenderer trên tất cả các child
        MeshRenderer[] renderers = currentBuilding.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer != null)
            {
                if (isPositionValid)
                {
                    renderer.material.color = Color.green;  // Màu xanh khi hợp lệ
                }
                else
                {
                    renderer.material.color = Color.red;  // Màu đỏ khi không hợp lệ
                }
            }
        }
    }

    private void HandleBuildingPlacement()
    {
        if (currentBuilding == null) return;

        // Nhấn chuột trái để xác nhận xây dựng
        if (Input.GetKeyDown(KeyCode.K) && isPositionValid)
        {
            Instantiate(buildingPrefab, currentBuilding.transform.position, currentBuilding.transform.rotation);
            Destroy(currentBuilding);  // Sau khi xây dựng, xóa vật thể đang di chuyển
        }
    }
}
