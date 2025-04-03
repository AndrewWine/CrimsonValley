using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestState : PlayerState
{

    public override void Enter()
    {
        base.Enter();
        blackboard.harvestButtonPress = false;
        blackboard.animator.Play("Harvest");
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if (isAnimationFinished)
           stateMachine.ChangeState(blackboard.idlePlayer);

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
