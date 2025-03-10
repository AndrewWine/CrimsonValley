using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HoeState : PlayerState
{

    
    public override void Enter()
    {
        base.Enter();
        blackboard.hoeButtonPressed = false;
        blackboard.animator.Play("Hoe");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            stateMachine.ChangeState(blackboard.idlePlayer);
        }

    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }



}
