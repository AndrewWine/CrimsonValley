using UnityEngine;

public class HoeSphereTrigger : MonoBehaviour
{
    [SerializeField] private GameObject CroptilePrefabs;
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider không được tìm thấy trên HoeSphere!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HoeSphere dang chay");

        if (other.CompareTag("Ground"))
        {
            Debug.Log("HoeSphere đang ở trên Ground");

            // Tính toán vùng kiểm tra dựa trên BoxCollider
            Vector3 boxCenter = boxCollider.bounds.center;
            Vector3 boxSize = boxCollider.size * 0.5f; // Kích thước nửa để phù hợp với OverlapBox
            Quaternion boxRotation = transform.rotation;

            // Kiểm tra nếu không có CropTile trong vùng kiểm tra
            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize, boxRotation);
            bool isCropTileNearby = false;

            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("CropTile"))
                {
                    isCropTileNearby = true;
                    Debug.Log("Có CropTile gần đó: " + hit.gameObject.name);
                    break;
                }
            }

            if (!isCropTileNearby)
            {
                Debug.Log("Không có CropTile gần đó, tạo mới.");
                Instantiate(CroptilePrefabs, transform.position, Quaternion.identity);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("HoeSphere dang chay");

        if (other.CompareTag("Ground"))
        {
            Debug.Log("HoeSphere đang ở trên Ground");

            // Tính toán vùng kiểm tra dựa trên BoxCollider
            Vector3 boxCenter = boxCollider.bounds.center;
            Vector3 boxSize = boxCollider.size * 0.5f; // Kích thước nửa để phù hợp với OverlapBox
            Quaternion boxRotation = transform.rotation;

            // Kiểm tra nếu không có CropTile trong vùng kiểm tra
            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize, boxRotation);
            bool isCropTileNearby = false;

            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("CropTile"))
                {
                    isCropTileNearby = true;
                    Debug.Log("Có CropTile gần đó: " + hit.gameObject.name);
                    break;
                }
            }

            if (!isCropTileNearby)
            {
                Debug.Log("Không có CropTile gần đó, tạo mới.");
                Instantiate(CroptilePrefabs, transform.position, Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Vẽ vùng kiểm tra hình hộp để debug
        if (boxCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}
