using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;   

public class PlayerAnimationEvents : MonoBehaviour
{
    // đặt vào player renderer trong editor
    [Header(" Elements ")]
    [SerializeField] private ParticleSystem seedParticles;
    [SerializeField] private ParticleSystem waterParticles;


    [Header("Events")]
    [SerializeField] private UnityEvent startHarvestingEvent;
    [SerializeField] private UnityEvent stopHarvestingEvent;

    private void PlayerSeedParticles()
    {
        seedParticles.Play();
    }

    private void PlayerWaterParticles()
    {
        waterParticles.Play();
    }

    private void StartHarvestingCallback()
    {
        startHarvestingEvent?.Invoke();
    }

    private void StopHarvestingCallback()
    {
        stopHarvestingEvent?.Invoke();

    }
}

