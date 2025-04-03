using UnityEngine;

public class NPCSchedule : MonoBehaviour
{
    public Transform[] destinations;  // Các điểm đến
    public float speed = 3f;  // Tốc độ di chuyển của NPC
    private int currentDestinationIndex = 0;  // Điểm đến hiện tại

    void Start()
    {
        // Thiết lập điểm đến ban đầu
        if (destinations.Length > 0)
        {
            MoveToNextDestination();
        }
    }

    void Update()
    {
        // Nếu NPC đã đến điểm đến
        if (Vector3.Distance(transform.position, destinations[currentDestinationIndex].position) < 0.5f)
        {
            // Di chuyển đến điểm tiếp theo trong danh sách
            currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Length;
            MoveToNextDestination();
        }

        // Di chuyển đến điểm hiện tại
        Vector3 direction = (destinations[currentDestinationIndex].position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // Hàm để di chuyển đến điểm tiếp theo
    private void MoveToNextDestination()
    {
        if (destinations.Length > 0)
        {
            Vector3 direction = (destinations[currentDestinationIndex].position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }
}
