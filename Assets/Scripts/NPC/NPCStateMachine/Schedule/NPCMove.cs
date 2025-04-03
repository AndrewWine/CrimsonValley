using UnityEngine;

public class NPCMove : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed = 3f; // Tốc độ di chuyển của NPC

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Phương thức di chuyển NPC đến vị trí đích
    public void MoveTo(Vector3 destination)
    {
        // Di chuyển với Rigidbody
        Vector3 direction = (destination - transform.position).normalized;
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }
}
