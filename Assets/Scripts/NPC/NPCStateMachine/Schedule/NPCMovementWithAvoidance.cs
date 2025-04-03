using UnityEngine;

public class NPCMovementWithAvoidance : MonoBehaviour
{
    private NPCScheduleManager scheduleManager;
    private Transform target; // Điểm đến lấy từ schedule
    public float speed = 3f;
    public float detectionRange = 2f;
    public float stoppingDistance = 0.5f;

    void Start()
    {
        // Tìm NPCScheduleManager trong cùng GameObject
        scheduleManager = GetComponent<NPCScheduleManager>();
    }

    void Update()
    {
       
        if (target != null)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        // Kiểm tra vật cản bằng raycast
        if (Physics.Raycast(transform.position, direction, detectionRange))
        {
            // Nếu có vật cản, thay đổi hướng (quay sang phải)
            direction = Quaternion.Euler(0, 90, 0) * direction;
        }

        // Di chuyển NPC
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Kiểm tra nếu NPC đã gần điểm đến
        if (Vector3.Distance(transform.position, target.position) < stoppingDistance)
        {
            return;
        }
    }
}
