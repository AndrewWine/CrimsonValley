using System;
using UnityEngine;

public class NPCScheduleManager : MonoBehaviour
{
    [Header("Cấu hình lịch biểu cho NPC")]
    public NPCScheduleData scheduleData;

    [Header("Liên kết với hệ thống quản lý trạng thái và Blackboard")]
    public NPCStateMachine stateMachine;
    public NPCBlackboard blackboard;
    private Transform targetTransform;
    private int currentScheduleIndex = -1;
    [SerializeField] private bool isScheduleComplete = true; // Chỉ cho phép đổi lịch khi hoàn thành

    private void Start()
    {
        // InvokeRepeating(nameof(CheckSchedule), 1f, 5f);
    }

    private void LateUpdate()
    {
        CheckSchedule();
    }

    private void CheckSchedule()
    {
        float currentTime = blackboard.dayNightCycle.GetCurrentHour();

        // Lấy lịch trình hiện tại theo giờ
        NPCScheduleData.ScheduleEntry newEntry = GetCurrentScheduleEntry(scheduleData, currentTime);

        // Nếu có lịch trình mới và khác lịch trình hiện tại, cập nhật nó
        if (newEntry != null && currentScheduleIndex != Array.IndexOf(scheduleData.scheduleEntries, newEntry))
        {
            currentScheduleIndex = Array.IndexOf(scheduleData.scheduleEntries, newEntry);
            //Debug.Log($"[NPCSchedule] Đổi trạng thái NPC sang: {newEntry.desiredState} vào {newEntry.startTime}h.");
            SwitchToSchedule(newEntry);
        }
    }


    public NPCScheduleData currentSchedule(NPCScheduleData scheduleData)
    {
        return scheduleData;
    }

    public void OnScheduleCompleted()
    {
        isScheduleComplete = true; // Cho phép cập nhật lịch trình mới khi đã hoàn thành
        Debug.Log("[NPCSchedule] Lịch trình đã hoàn tất, sẵn sàng chuyển sang lịch trình mới.");
    }

    public NPCScheduleData.ScheduleEntry GetCurrentScheduleEntry(NPCScheduleData scheduleData, float currentTime)
    {
        NPCScheduleData.ScheduleEntry latestEntry = null;

        for (int i = 0; i < scheduleData.scheduleEntries.Length; i++)
        {
            var entry = scheduleData.scheduleEntries[i];

            // Nếu đã đến giờ startTime của một entry mới, lấy entry này
            if (currentTime >= entry.startTime)
            {
                latestEntry = entry;
            }
        }

        return latestEntry; // Trả về entry mới nhất có startTime <= currentTime
    }


    private void SwitchToSchedule(NPCScheduleData.ScheduleEntry entry)
    {
        if (targetTransform == null || targetTransform.name != entry.targetName)
        {
            targetTransform = FindTargetTransform(entry.targetName);
            if (targetTransform == null)
            {
                Debug.LogError($"Không tìm thấy target '{entry.targetName}' trong scene!");
                return;
            }
        }

        if (entry.desiredState == NPCStateType.Move)
        {
            if (blackboard.nPCMove != null)
            {
                NPCMoveState moveState = blackboard.nPCMove as NPCMoveState;
                if (moveState != null)
                {
                    moveState.SetDestination(targetTransform, this); // Truyền chính `NPCScheduleManager`
                    stateMachine.ChangeState(moveState);
                }
            }
        }
        else if (entry.desiredState == NPCStateType.Idle)
        {
            stateMachine.ChangeState(blackboard.nPCIdle);

        }
        else if (entry.desiredState == NPCStateType.PlantTree)
        {
            stateMachine.ChangeState(blackboard.nPCPlantTree);

        }
        else if (entry.desiredState == NPCStateType.Chop)
        {
            stateMachine.ChangeState(blackboard.nPCChop);

        }
        else if (entry.desiredState == NPCStateType.PickAxe)
        {
            stateMachine.ChangeState(blackboard.nPCPickAxe);

        }

    }

    public Transform GetTargetTransform()
    {
        return targetTransform;
    }

    private Transform FindTargetTransform(string targetName)
    {
        GameObject targetObject = GameObject.Find(targetName);
        return targetObject ? targetObject.transform : null;
    }
}
