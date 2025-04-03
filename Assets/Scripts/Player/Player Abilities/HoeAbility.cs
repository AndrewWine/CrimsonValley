using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;
public class HoeAbility : MonoBehaviour
{
    // Start is called before the first frame update
    [Header(" Elements ")]
    [SerializeField] private Transform hoeSphere;
    [SerializeField] private GameObject CropFieldPrefab;
    [SerializeField] private BoxCollider HoeColider;
    [SerializeField] private LineRenderer lineRenderer;
    private PlayerBlackBoard blackboard;
    [Header(" Settings ")]
    private CropTile currentCropTile;
    [SerializeField]private bool canHoe;
    bool isCropFieldNearby = false;
    bool canCreateCropTile = true;
    private Vector3 lowerPosition;


    void Start()
    {
       
        blackboard = GetComponentInParent<PlayerBlackBoard>();
        SetupLineRenderer();

        AnimationTriggerCrop.onCreateACropField += CreateCropfield;
    }


    void Update()
    {
        if (blackboard.playerToolSelector.CanHoe())
        {
            lineRenderer.enabled = true;
            DrawOutline(); // Luôn cập nhật khung BoxCollider
        }
        else
        {
            lineRenderer.enabled = false;
        }


        lowerPosition = hoeSphere.transform.position - new Vector3(0, 0.6f, 0);


    }

    private void OnDisable()
    {

        AnimationTriggerCrop.onCreateACropField -= CreateCropfield;

    }



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("FarmArea"))
        {
            canHoe = true;
            // Lấy bán kính từ BoxCollider của hoeSphere
            BoxCollider boxCollider = hoeSphere.GetComponent<BoxCollider>();
            Vector3 checkSize = boxCollider != null ? boxCollider.size * 0.5f : Vector3.one * 0.5f;

            // Kiểm tra nếu hoeSphere không chạm vào bất kỳ CropTile nào
            Collider[] hitColliders = Physics.OverlapBox(hoeSphere.transform.position, checkSize, Quaternion.identity);
            isCropFieldNearby = false;

            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("Croptile"))
                {
                    isCropFieldNearby = true;
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FarmArea"))
        {
            canHoe = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FarmArea"))
        {
            blackboard.isGround = false;

        }
    }


    private void CreateCropfield()
    {
        if (!canHoe) return; // Không thể cuốc đất thì thoát ngay

        if (CropFieldPrefab == null)
        {
            Debug.LogError(" CropFieldPrefab bị null! Kiểm tra Prefab trong Inspector.");
            return;
        }

        // Lấy vị trí cuối cùng của LineRenderer (góc trước của HoeColider)
        Vector3 targetPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

        // Giữ nguyên Y từ lowerPosition để đảm bảo đúng mặt đất
        float tileSize = 1.0f;
        Vector3 snappedPosition = new Vector3(
            Mathf.Round(targetPosition.x / tileSize) * tileSize,
            lowerPosition.y, // Giữ nguyên Y
            Mathf.Round(targetPosition.z / tileSize) * tileSize
        );

        // Kiểm tra xem vị trí này đã có CropTile chưa
        if (IsPositionOccupied(snappedPosition))
        {
            Debug.Log(" Vị trí đã có CropTile, không thể tạo mới.");
            return;
        }

        // Instantiate CropField
        GameObject newCropField = Instantiate(CropFieldPrefab, snappedPosition, Quaternion.identity);

        if (newCropField == null)
        {
            Debug.LogError(" Lỗi khi Instantiate CropFieldPrefab, đối tượng bị null!");
            return;
        }

        // Lấy component Item từ đối tượng vừa Instantiate
        Item itemComponent = newCropField.GetComponent<Item>();

        if (itemComponent == null)
        {
            Debug.LogError(" CropFieldPrefab KHÔNG có component Item! Kiểm tra Prefab.");
            return;
        }

        // Gửi sự kiện EventBus
        EventBus.Publish(new ItemPlacedEvent(itemComponent));
        Debug.Log(" Đã gửi sự kiện ItemPlacedEvent cho: " + itemComponent.name);

        // Tiêu hao stamina
        PlayerStatusManager.Instance.UseStamina(2);
    }







    /// <summary>
    /// Kiểm tra xem một vị trí đã có CropTile hay chưa.
    /// </summary>
    /// <param name="position">Vị trí cần kiểm tra.</param>
    /// <returns>True nếu đã có CropTile, False nếu chưa.</returns>
    private bool IsPositionOccupied(Vector3 position)
    {
        // Kiểm tra có Collider nào với tag "Croptile" tại vị trí này không
        Collider[] colliders = Physics.OverlapSphere(position, 0.4f); // Bán kính kiểm tra nhỏ để tránh lỗi
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Croptile"))
            {
                return true; // Đã có CropTile tại vị trí này
            }
        }
        return false; // Vị trí còn trống
    }


    void SetupLineRenderer()
    {
        lineRenderer.positionCount = 24; // 12 cạnh của BoxCollider
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.yellow;
    }


    void DrawOutline()
    {
        if (HoeColider == null || lineRenderer == null) return;

        Vector3 colliderSize = new Vector3(HoeColider.size.x * 0.5f, HoeColider.size.y * 0.01f, HoeColider.size.z * 0.5f);
        float fixedY = hoeSphere.position.y;

        // Lấy hướng nhìn của nhân vật
        Vector3 lookDirection = blackboard.animator.transform.forward;
        lookDirection.y = 0;
        lookDirection.Normalize();

        // Xác định vị trí mới của hoeSphere phía trước nhân vật
        Vector3 hoePosition = blackboard.transform.position + lookDirection * 1.5f;
        hoePosition.y = fixedY;

        // Xoay BoxCollider theo hướng nhìn
        Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        // Các góc của BoxCollider
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

        // Xoay và dịch chuyển các góc
        for (int i = 0; i < localCorners.Length; i++)
        {
            localCorners[i] = rotation * localCorners[i]; // Xoay
            localCorners[i] += hoePosition; // Dịch vị trí ra trước mặt nhân vật
        }

        // Vẽ khung
        Vector3[] edges = new Vector3[]
        {
        localCorners[0], localCorners[1],
        localCorners[1], localCorners[2],
        localCorners[2], localCorners[3],
        localCorners[3], localCorners[0],

        localCorners[4], localCorners[5],
        localCorners[5], localCorners[6],
        localCorners[6], localCorners[7],
        localCorners[7], localCorners[4],

        localCorners[0], localCorners[4],
        localCorners[1], localCorners[5],
        localCorners[2], localCorners[6],
        localCorners[3], localCorners[7]
        };

        lineRenderer.positionCount = edges.Length;
        lineRenderer.SetPositions(edges);
    }





}