using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CutState : PlayerState
{
    [Header("Actions")]
    public static Action notifyCutting;
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if(isAnimationFinished)
            notifyCutting?.Invoke();//CuttingAbillity
            stateMachine.ChangeState(blackboard.idlePlayer);
    }

    public override void Enter()
    {
        base.Enter();
        blackboard.cutButtonPressed = false;
        blackboard.animator.Play("Cutting");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
}
