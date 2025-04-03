using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAwakeState : NPCState
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("NPC bắt đầu ngủ, tắt GameObject...");
        gameObject.SetActive(true); 
    }

    public override void Exit()
    {
        base.Exit();
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
