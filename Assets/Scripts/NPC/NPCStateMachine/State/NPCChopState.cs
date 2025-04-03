using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCChopState : NPCState
{
    public override void Enter()
    {
        base.Enter();
        blackboard.animator.Play("Chop");
    }
}
