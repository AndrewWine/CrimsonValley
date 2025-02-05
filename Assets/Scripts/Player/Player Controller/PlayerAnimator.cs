using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header(" Elements")]
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem waterParticles;

    private MobileJoystick joystick;

    [Header(" Settings ")]
    [SerializeField] private float moveSpeedMultiplier;
    private float runThreshold = 0.2f;


    [Header("Actions")]
    public Action createTile;
    public bool canHoe;


    // Start is called before the first frame update
    void Start()
    {
        joystick = GetComponentInChildren<MobileJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ManageAnimations(Vector3 moveVector)
    {
        if (moveVector.magnitude > 0)
        {
            animator.SetFloat("moveSpeed", moveVector.magnitude * moveSpeedMultiplier);
            animator.transform.forward = moveVector.normalized;

            if (moveVector.magnitude * moveSpeedMultiplier >= runThreshold)
            {
                PlayRunAnimation();
            }
            else
            {
                PlayWalkAnimation();
            }
        }
        else
        {
            PlayIdleAnimation();
        }
    }

    private void PlayIdleAnimation()
    {
        animator.Play("Idle");
    }

    private void PlayRunAnimation()
    {
        animator.Play("Run");
    }

    private void PlayWalkAnimation()
    {
        animator.Play("Walking");
    }
    
    public void PlaySowAnimation()
    {
        animator.SetLayerWeight(1,1);
    }

    public void StopSowAnimation()
    {
        animator.SetLayerWeight(1, 0);
    }

    public void StopWaterAnimation()
    {
        animator.SetLayerWeight(2, 0);
        waterParticles.Stop();
    }
    public void PlayWaterAnimation()
    {
        animator.SetLayerWeight(2, 1);
    }

    public void PlayHarvestAnimation()
    {
        animator.SetLayerWeight(3, 1);
    }
    public void StopHarvestAnimation()
    {
        animator.SetLayerWeight(3, 0);
    }

    public void PlayHoeAnimation()
    {
        animator.SetLayerWeight(4, 1);
        // Bật cờ canHoe khi animation bắt đầu
        canHoe = true;
    }

    public void StopHoeAnimation()
    {
        canHoe = false;
        animator.SetLayerWeight(4, 0);
    }

    public void PlayCuttingAnimation()
    {
        animator.SetLayerWeight(5, 1);
        // Bật cờ canHoe khi animation bắt đầu
    }

    public void StopCuttingAnimation()
    {
        animator.SetLayerWeight(5, 0);
    }

}
