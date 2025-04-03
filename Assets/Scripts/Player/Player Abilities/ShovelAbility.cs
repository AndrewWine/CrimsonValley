using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShovelAbility : MonoBehaviour
{
    [Header("Elements")]
    private CheckGameObject checkGameObject;
    [SerializeField] private Transform colliderTransform;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private LineRenderer lineRenderer;
    private PlayerBlackBoard blackboard;

    void Start()
    {
        blackboard = GetComponentInParent<PlayerBlackBoard>();
        checkGameObject = GetComponentInChildren<CheckGameObject>();
        SetupLineRenderer();
    }

    private void OnEnable()
    {
        ActionButton.Shoveling += OnDestroyGameObject;
    }

    private void OnDisable()
    {
        ActionButton.Shoveling -= OnDestroyGameObject;
    }

    void Update()
    {
        if (blackboard.playerToolSelector.CanShovel())
        {
            lineRenderer.enabled = true;
            UpdateColliderPosition(); // Luôn cập nhật vị trí BoxCollider
            DrawOutline(); // Luôn cập nhật khung hộp
        }
        else
        {
            lineRenderer.enabled = false;
        }

        // Đổi màu LineRenderer theo điều kiện
        lineRenderer.material.color = checkGameObject.currentGameObject ? Color.red : Color.green;
    }



    public void OnDestroyGameObject()
    {
        if (checkGameObject.currentGameObject == null) return;

        GameObject objToDestroy = checkGameObject.currentGameObject;

        if (checkGameObject.buildingGameObject || checkGameObject.cropTile)
        {
            Item item = objToDestroy.GetComponent<Item>();
            if (item != null && checkGameObject.cropTile)
            {
                // Xóa trực tiếp từ placedItems
                WorldManager.instance.worldData.placedItems.RemoveAll(i =>
                    i.itemName == item.itemData.itemName &&
                    Vector3.Distance(i.position, item.transform.position) < 0.01f);
            }

            PlacedBuilding building = objToDestroy.GetComponent<PlacedBuilding>();
            if (building != null && checkGameObject.buildingGameObject)
            {
                // Xóa trực tiếp từ placedBuildings
                WorldManager.instance.worldData.placedBuildings.RemoveAll(b =>
                    b.buildingName == building.buildingData.buildingName &&
                    Vector3.Distance(b.position, building.transform.position) < 0.01f);
            }
        }

        // Đặt gameObject thành null trước khi gọi SaveWorld
        checkGameObject.currentGameObject = null;

        // Lưu game trước khi Destroy
        WorldManager.instance.SaveWorld();

        // Hủy game object
        Destroy(objToDestroy);
    }











    void SetupLineRenderer()
    {
        lineRenderer.positionCount = 24; // 12 cạnh của hình hộp
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
    }

    void UpdateColliderPosition()
    {
        // Lấy hướng nhìn của nhân vật
        Vector3 lookDirection = blackboard.animator.transform.forward;
        lookDirection.y = 0;
        lookDirection.Normalize();

        // Xác định vị trí mới cho BoxCollider
        Vector3 newPosition = blackboard.transform.position + lookDirection * 1.5f;
        newPosition.y = colliderTransform.position.y; // Giữ nguyên độ cao

        // Cập nhật vị trí của colliderTransform
        colliderTransform.position = newPosition;
        colliderTransform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }

    void DrawOutline()
    {
        if (boxCollider == null || lineRenderer == null) return;

        Vector3 colliderSize = new Vector3(boxCollider.size.x * 0.5f, boxCollider.size.y * 0.01f, boxCollider.size.z * 0.5f);
        Vector3 hoePosition = colliderTransform.position;
        Quaternion rotation = colliderTransform.rotation;

        // Tạo danh sách các đỉnh của BoxCollider
        Vector3[] localCorners = new Vector3[8]
        {
            new Vector3(-colliderSize.x, -colliderSize.y, -colliderSize.z),
            new Vector3(colliderSize.x, -colliderSize.y, -colliderSize.z),
            new Vector3(colliderSize.x, -colliderSize.y, colliderSize.z),
            new Vector3(-colliderSize.x, -colliderSize.y, colliderSize.z),

            new Vector3(-colliderSize.x, colliderSize.y, -colliderSize.z),
            new Vector3(colliderSize.x, colliderSize.y, -colliderSize.z),
            new Vector3(colliderSize.x, colliderSize.y, colliderSize.z),
            new Vector3(-colliderSize.x, colliderSize.y, colliderSize.z)
        };

        // Áp dụng xoay và di chuyển
        for (int i = 0; i < localCorners.Length; i++)
        {
            localCorners[i] = rotation * localCorners[i] + hoePosition;
        }

        // Kết nối các cạnh để vẽ hình hộp
        Vector3[] edges = new Vector3[]
        {
            localCorners[0], localCorners[1], localCorners[1], localCorners[2], localCorners[2], localCorners[3], localCorners[3], localCorners[0],
            localCorners[4], localCorners[5], localCorners[5], localCorners[6], localCorners[6], localCorners[7], localCorners[7], localCorners[4],
            localCorners[0], localCorners[4], localCorners[1], localCorners[5], localCorners[2], localCorners[6], localCorners[3], localCorners[7]
        };

        lineRenderer.positionCount = edges.Length;
        lineRenderer.SetPositions(edges);
    }
}
