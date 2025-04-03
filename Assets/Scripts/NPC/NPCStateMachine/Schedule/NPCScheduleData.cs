using UnityEngine;

public enum NPCStateType
{
    Idle,
    Move,
    Chop,
    PickAxe,
    PlantTree
}

[CreateAssetMenu(fileName = "NPCScheduleData", menuName = "NPC/Schedule Data")]
public class NPCScheduleData : ScriptableObject
{
    [System.Serializable]
public class ScheduleEntry
{
    [Tooltip("Time begin")]
    public float startTime;

    [Tooltip("State NPC switch")]
    public NPCStateType desiredState;

    [Tooltip("If MoveState, use target name to find transform in scene")]
    public string targetName; // Lưu tên của điểm đến thay vì Transform
}


    [Header("Danh sách lịch biểu (sắp xếp theo thời gian tăng dần)")]
    public ScheduleEntry[] scheduleEntries;
}
