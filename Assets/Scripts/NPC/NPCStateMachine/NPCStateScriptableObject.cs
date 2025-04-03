using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCState", menuName = "NPC/State", order = 0)]
public class NPCStateScriptableObject : ScriptableObject
{
    public string stateName;  // Tên trạng thái của NPC

    public virtual void Enter(NPCBlackboard blackboard)
    {
        // Được gọi khi vào trạng thái
    }

    public virtual void Exit(NPCBlackboard blackboard)
    {
        // Được gọi khi thoát trạng thái
    }

    public virtual void LogicUpdate(NPCBlackboard blackboard)
    {
        // Được gọi trong Update()
    }

    public virtual void PhysicUpdate(NPCBlackboard blackboard)
    {
        // Được gọi trong FixedUpdate()
    }
}
