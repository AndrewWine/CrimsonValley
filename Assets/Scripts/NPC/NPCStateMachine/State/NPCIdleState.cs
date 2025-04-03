using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdle : NPCState
{
    public override void Enter()
    {
        base.Enter();

        if (blackboard == null)
        {
            
            return;
        }

        if (blackboard.animator == null)
        {
            Debug.LogError("Animator is null in NPCBlackboard!");
            return;
        }

        blackboard.animator.Play("NPCIdle");
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();
    
        
    }
}
