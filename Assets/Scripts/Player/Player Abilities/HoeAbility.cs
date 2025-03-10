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
    private bool isGround;
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


        lowerPosition = hoeSphere.transform.position - new Vector3(0, 0.5f, 0);


    }

    private void OnDisable()
    {

        AnimationTriggerCrop.onCreateACropField -= CreateCropfield;

    }



    private void OnCollisionStay(Collision collision)
    {
        // Kiểm tra nếu va chạm với "Ground" và player có thể hoe
        if (collision.gameObject.CompareTag("Ground"))
        {
            blackboard.isGround = true;

            // Lấy bán kính từ BoxCollider của hoeSphere
            BoxCollider boxCollider = hoeSphere.GetComponent<BoxCollider>();
            Vector3 checkSize = boxCollider != null ? boxCollider.size * 0.5f : Vector3.one * 0.5f; // Nếu không tìm thấy BoxCollider, dùng kích thước mặc định

            // Kiểm tra nếu hoeSphere không chạm vào bất kỳ CropTile nào
            Collider[] hitColliders = Physics.OverlapBox(hoeSphere.transform.position, checkSize, Quaternion.identity);
            isCropFieldNearby = false; // Mặc định không có CropTile

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            blackboard.isGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            blackboard.isGround = false;
        }
    }


    private void CreateCropfield()
    {
        if (!isGround)
        {
            // Làm tròn vị trí để khớp lưới
            float tileSize = 1.0f;
            Vector3 snappedPosition = new Vector3(
                Mathf.Round(lowerPosition.x / tileSize) * tileSize,
                lowerPosition.y,
                Mathf.Round(lowerPosition.z / tileSize) * tileSize
            );

            // Nếu vị trí này chưa có CropTile, tạo mới
            if (!IsPositionOccupied(snappedPosition))
            {
                Instantiate(CropFieldPrefab, snappedPosition, Quaternion.identity);
            }
        }  
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

        // Lấy vị trí gốc của Box Collider
        Vector3 colliderCenter = HoeColider.center;
        Vector3 colliderSize = HoeColider.size * 0.5f; // Lấy 1/2 kích thước

        // Lấy giá trị Y cố định (giữ nguyên Y của hoeSphere)
        float fixedY = hoeSphere.position.y;

        // Đảm bảo LineRenderer theo đúng hoeSphere
        lineRenderer.transform.position = hoeSphere.position;
        lineRenderer.transform.rotation = hoeSphere.rotation;

        // Tính toán góc của Box Collider dựa trên center
        Vector3[] localCorners = new Vector3[8]
        {
        new Vector3(-colliderSize.x, -colliderSize.y, -colliderSize.z) + colliderCenter,
        new Vector3(colliderSize.x, -colliderSize.y, -colliderSize.z) + colliderCenter,
        new Vector3(colliderSize.x, -colliderSize.y, colliderSize.z) + colliderCenter,
        new Vector3(-colliderSize.x, -colliderSize.y, colliderSize.z) + colliderCenter,

        new Vector3(-colliderSize.x, colliderSize.y, -colliderSize.z) + colliderCenter,
        new Vector3(colliderSize.x, colliderSize.y, -colliderSize.z) + colliderCenter,
        new Vector3(colliderSize.x, colliderSize.y, colliderSize.z) + colliderCenter,
        new Vector3(-colliderSize.x, colliderSize.y, colliderSize.z) + colliderCenter
        };

        for (int i = 0; i < localCorners.Length; i++)
        {
            localCorners[i] = HoeColider.transform.TransformPoint(localCorners[i]);
            localCorners[i].y = fixedY; // Giữ nguyên Y
        }

        // Vẽ các cạnh
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