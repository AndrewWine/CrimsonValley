﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class Cage : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] protected Inventory inventory;
    [SerializeField] public CageState state;


    [Header("Settings")]
    [SerializeField] protected int nutrition;
    [SerializeField] protected int harvestTimer;
    [SerializeField] protected int slot;
    [SerializeField] protected float feedingTimer;
    [SerializeField] protected int DigestionTime = 10;
    protected float timeToHarvestProductPassed = 0;
    [Header("QuantityItems")]
    [SerializeField] protected int quantity;
    [SerializeField] protected string nameOfMaterial;

    protected virtual void OnEnable() 
    { 
        InitializeSetting();
        CheckUICageStatus.FeedButton += ReduceTimeProduce;
        CheckUICageStatus.TakeProduceButton += ResetTakeProduceTimer;
    }

    protected virtual void OnDestroy()
    {
        CheckUICageStatus.FeedButton -= ReduceTimeProduce;
        CheckUICageStatus.TakeProduceButton -= ResetTakeProduceTimer;
    }

    private void InitializeSetting()
    {
        feedingTimer = DigestionTime;
        timeToHarvestProductPassed = harvestTimer;
    }


    protected virtual void Update()
    {
        timeCounter();
    }

    protected virtual void CheckHarvestProduce()
    {
        state = CageState.TakeProduce;
    }
    protected virtual void ChangeFeedStatusOfAnimals()
    {
        state = CageState.Hungry;
    }

    private void timeCounter()
    {
        feedingTimer -= Time.deltaTime;
        timeToHarvestProductPassed -= Time.deltaTime;
        if (feedingTimer < 0) ChangeFeedStatusOfAnimals();
        if (timeToHarvestProductPassed < 0) CheckHarvestProduce();
    }

    public virtual void ReduceTimeProduce(int nutrition)
    {
        if (nutrition > 0)
        {
            // Giảm thời gian thu hoạch
            timeToHarvestProductPassed -= nutrition;
            feedingTimer = DigestionTime;
          
        }
        else
        {
            Debug.LogWarning("Invalid nutrition value.");
        }
    }

    // Phương thức dùng chung cho tất cả các lớp con
    protected virtual void ResetTakeProduceTimer()
    {
        timeToHarvestProductPassed = harvestTimer;
       

    }
}
