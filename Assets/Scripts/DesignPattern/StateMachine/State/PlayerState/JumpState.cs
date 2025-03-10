using UnityEngine;

public class JumpState : PlayerState
{
    [SerializeField] private MobileJoystick joystick; // Joystick di động

    private bool isJumping; // Biến để kiểm tra trạng thái nhảy

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        // Kiểm tra animation đã hoàn thành và chuyển trạng thái
        if (isAnimationFinished)
        {
            stateMachine.ChangeState(blackboard.idlePlayer);
        }
    }

    public override void Enter()
    {
        base.Enter();
        // Phát animation nhảy
        blackboard.jumpButtonPressed = false;
        blackboard.animator.Play("Jumping");
    }

    // Di chuyển qua lại trong khi nhảy
   

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (joystick != null && joystick.GetMoveVector().magnitude > 0.1f)
        {
            stateMachine.ChangeState(blackboard.moveState);
        }
    }
}
