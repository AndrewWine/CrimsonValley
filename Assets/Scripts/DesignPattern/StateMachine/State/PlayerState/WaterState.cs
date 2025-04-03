    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterState : PlayerState
{


    public override void Enter()
    {
        base.Enter();
        blackboard.waterButtonPressed = false;
        OnWaterButtonPressed();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
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

   

    public void OnWaterButtonPressed()
    {
        blackboard.animator.Play("Watering");
        // Lấy CropField duy nhất mà người chơi đang va chạm
        CropField currentCropField = FindObjectOfType<CheckGameObject>().GetCurrentCropField();

        // Chỉ gieo hạt cho CropField mà người chơi đang va chạm và có trạng thái Empty
        if (currentCropField != null && currentCropField.state == TileFieldState.Sown)
        {
            Debug.Log("Sowing CropField: " + currentCropField.name);

            // Gọi trực tiếp hàm Sow() cho mỗi cropTile trong CropField hiện tại
            foreach (var cropTile in currentCropField.GetTiles())
            {
                cropTile.Water();
            }

            currentCropField.FieldFullyWatered(); // Đảm bảo cập nhật state
        }
        else
        {
            Debug.Log("No valid CropField to sow");
        }
    }
}
