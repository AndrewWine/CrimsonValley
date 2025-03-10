using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MiningState : PlayerState
{
    [Header("Elements")]
    [SerializeField] private Transform CheckGameObject;
    [Header("Actions")]
    public static Action NotifyMining;
    

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if (isAnimationFinished)
        {
            // Lấy OreRock từ CheckCropFieldState đã được gán trong OnTriggerEnter
            NotifyMining?.Invoke();//MiningAbility
            stateMachine.ChangeState(blackboard.idlePlayer);
        }
    }

    public override void Enter()
    {
        base.Enter();
        blackboard.miningButtonPressed = false;
        blackboard.animator.Play("Mining");
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
