using System;
using UnityEngine;

public class SowState : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        // Bật một trigger để kiểm tra animation kết thúc
        blackboard.animator.Play("Sow");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void OnSowButtonPressed()
    {
        blackboard.sowButtonPressed = false;

        CropField currentCropField = FindObjectOfType<CheckGameObject>()?.GetCurrentCropField();

       

        currentCropField.Sow(blackboard.seed);
        stateMachine.ChangeState(blackboard.idlePlayer); // Sau khi gieo hạt xong, chuyển về trạng thái idle

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        // Khi animation kết thúc, chuyển trạng thái về idle
        if (isAnimationFinished)
        {
            OnSowButtonPressed();

        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
