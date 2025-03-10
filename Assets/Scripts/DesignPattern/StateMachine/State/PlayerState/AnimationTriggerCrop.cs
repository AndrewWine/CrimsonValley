using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerCrop : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private PlayerStateMachine playerStateMachine;
    [SerializeField] private Transform hoeSphere;
    private int tilesHarvested;

    [Header("Actions")]
    public static Action onHarvestTriggered;
    public static Action onCreateACropField;
    public static Action onJumping;



    public void FinishAnimationTrigger()
    {
        if (playerStateMachine != null && playerStateMachine.CurrentState != null)
        {
            playerStateMachine.CurrentState.AnimationFinishTrigger();
        }
        else
        {
            Debug.LogError("Không thể gọi AnimationFinishTrigger vì CurrentState hoặc PlayerStateMachine là null.");
        }
    }

    public void Harvest()
    {
        Debug.Log("Sự kiện thu hoạch được kích hoạt!");

        onHarvestTriggered?.Invoke();//cropfield
    }

    public void Hoeing()
    {
        onCreateACropField?.Invoke();//HoeAbility
    }

    public void Jumping()
    {
        onJumping?.Invoke();//JumpAbility
        Debug.Log("Call jump");
    }
}
