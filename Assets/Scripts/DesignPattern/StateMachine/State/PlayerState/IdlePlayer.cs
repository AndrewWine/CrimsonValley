using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStatePlayer : PlayerState
{
    private MobileJoystick joystick;

    public override void Enter()
    {
        base.Enter();
        blackboard.animator.Play("Idle");

        // Tìm joystick nếu chưa có
        if (joystick == null)
        {
            joystick = GameObject.FindObjectOfType<MobileJoystick>();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Kiểm tra joystick có input không
        if (joystick != null && joystick.GetMoveVector().magnitude > 0.1f)
        {
            stateMachine.ChangeState(blackboard.moveState);
        }

        else if (blackboard.sowButtonPressed && blackboard.seed != null)
        {
            stateMachine.ChangeState(blackboard.sowState);
        }

        else if (blackboard.harvestButtonPress)
        {
            stateMachine.ChangeState(blackboard.harvestState);

        }

        else if (blackboard.waterButtonPressed)
        {
            stateMachine.ChangeState(blackboard.waterState);
        }
        else if (blackboard.hoeButtonPressed)
        {
            stateMachine.ChangeState(blackboard.hoeState);
        }
        else if (blackboard.cutButtonPressed && blackboard.isTree)
        {
            stateMachine.ChangeState(blackboard.cutState);
        }
        else if (blackboard.miningButtonPressed)
        {
            stateMachine.ChangeState(blackboard.miningState);
        }
        else if (blackboard.jumpButtonPressed)
        {
            stateMachine.ChangeState(blackboard.jumpState);
        }
        else if( blackboard.sleepButtonPressed)
        {
            stateMachine.ChangeState(blackboard.sleepState);
        }
        else if (blackboard.stamina <= 0)
        {
            stateMachine.ChangeState(blackboard.sleepState);
        }
    }
}
