using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPickAxeState : NPCState
{
    public override void Enter()
    {
        base.Enter();
        blackboard.animator.Play("Pickaxe");
    }
}
