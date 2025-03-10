using System;
using UnityEngine;

public class SowState : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        // Bật một trigger để kiểm tra animation kết thúc
        PlayerStatusManager.Instance.UseStamina(1); // Mỗi lần dùng công cụ trừ 10 Stamina
        blackboard.animator.Play("Sow");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void OnSowButtonPressed()
    {
        blackboard.sowButtonPressed = false;

        CropField currentCropField = FindObjectOfType<CheckCropFieldState>()?.GetCurrentCropField();

        if (currentCropField == null)
        {
            Debug.LogError("Không tìm thấy ô đất để gieo trồng!");
            return;
        }

        if (currentCropField.state != TileFieldState.Empty)
        {
            Debug.LogError("Ô đất không trống!");
            return;
        }

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
