using UnityEngine;

public class NPCMoveState : NPCState
{
    private Transform target;
    private NPCScheduleManager scheduleManager;
    public float speed = 3f;
    public float stoppingDistance = 1f;
    public float rotationSpeed = 10f;
    public float detectionRange = 2f;
    public float avoidanceAngle = 45f;
    public float maxStepHeight = 0.5f; // Chiều cao tối đa NPC có thể bước lên
    public float groundCheckDistance = 1.5f;
    public LayerMask Ground; // Chỉ nhận diện layer của terrain

    public override void Enter()
    {
        base.Enter();
        blackboard.animator.Play("NPCWalk");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (target != null)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        if (target == null) return;

        float currentTime = blackboard.dayNightCycle.GetCurrentHour();
        float distance = Vector3.Distance(blackboard.transform.position, target.position);

        if (distance <= stoppingDistance)
        {
            NPCScheduleData.ScheduleEntry entry = scheduleManager.GetCurrentScheduleEntry(scheduleManager.scheduleData, currentTime);

            if (entry != null && currentTime == entry.startTime)
            {
                scheduleManager.OnScheduleCompleted();
                Debug.Log("Đã tới điểm đến");
            }
            stateMachine.ChangeState(blackboard.nPCIdle);
            return;
        }

        Vector3 direction = (target.position - blackboard.transform.position).normalized;

        //  Kiểm tra vật cản phía trước
        if (Physics.Raycast(blackboard.transform.position, direction, out RaycastHit hit, detectionRange))
        {
            if (hit.collider.CompareTag("Tree"))
            {
                Debug.Log("có vật");
                Vector3 flatDirection = new Vector3(direction.x, 0, direction.z).normalized;
                Vector3 rightDir = Quaternion.Euler(0, avoidanceAngle, 0) * flatDirection;
                Vector3 leftDir = Quaternion.Euler(0, -avoidanceAngle, 0) * flatDirection;


                bool rightClear = !Physics.Raycast(blackboard.transform.position, rightDir, detectionRange);
                bool leftClear = !Physics.Raycast(blackboard.transform.position, leftDir, detectionRange);

                if (rightClear)
                    direction = rightDir;
                else if (leftClear)
                    direction = leftDir;
                else
                    direction = -direction;
            }
        }

        //  Kiểm tra địa hình phía trước
        Vector3 forwardCheckPos = blackboard.transform.position + direction * groundCheckDistance;

        if (Physics.Raycast(forwardCheckPos + Vector3.up * 2, Vector3.down, out RaycastHit groundHit, 4f, Ground))
        {
            Debug.Log("có thấy vật cản");

            float heightDifference = groundHit.point.y - blackboard.transform.position.y;

            if (heightDifference > maxStepHeight)
            {
                Debug.Log("đổi hướng");

                // Địa hình quá cao, thử né tránh sang trái/phải
                Vector3 rightDir = Quaternion.Euler(0, avoidanceAngle, 0) * direction;
                Vector3 leftDir = Quaternion.Euler(0, -avoidanceAngle, 0) * direction;

                bool rightClear = !Physics.Raycast(forwardCheckPos + rightDir * groundCheckDistance + Vector3.up * 2, Vector3.down, out RaycastHit rightHit, 4f, Ground) || (rightHit.point.y - blackboard.transform.position.y) <= maxStepHeight;
                bool leftClear = !Physics.Raycast(forwardCheckPos + leftDir * groundCheckDistance + Vector3.up * 2, Vector3.down, out RaycastHit leftHit, 4f, Ground) || (leftHit.point.y - blackboard.transform.position.y) <= maxStepHeight;

                if (rightClear)
                    direction = rightDir;
                else if (leftClear)
                    direction = leftDir;
                else
                    return; // Đứng yên nếu không có đường nào hợp lệ
            }
        }

        //  Giữ NPC luôn trên mặt đất
        if (Physics.Raycast(blackboard.transform.position + Vector3.up, Vector3.down, out RaycastHit groundHitNPC, 2f, Ground))
        {
            blackboard.transform.position = new Vector3(blackboard.transform.position.x, groundHitNPC.point.y, blackboard.transform.position.z);
        }

        //  Quay NPC theo hướng di chuyển
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            blackboard.transform.rotation = Quaternion.Slerp(blackboard.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        //  Di chuyển NPC
        blackboard.transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDestination(Transform newTarget, NPCScheduleManager manager)
    {
        if (newTarget == null)
        {
            Debug.LogError("Target is null!");
            return;
        }
        target = newTarget;
        scheduleManager = manager;
    }
}
