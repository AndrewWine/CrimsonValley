using System;
using UnityEngine;

public class Cage : MonoBehaviour
{
    [Header("Cage State")]
    [SerializeField] public CageState state;
    [SerializeField] protected Transform CagePos;
    [Header("Item Drop Settings")]
    [SerializeField] private ItemData itemDrop;
    [SerializeField] private int dropAmount = 10;

    [Header("Feeding & Production")]
    [SerializeField] private int nutritionValue;
    [SerializeField] private int harvestDuration;
    [SerializeField] private int digestionTime = 10;

    [SerializeField] private float timeUntilHarvest;
    [SerializeField] private float feedingTimer;

    [Header("Event Modifiers")]
    private int buffDropRate;
    private int debuffDropRate;

    protected virtual void Start()
    {
        InitializeCage();
        CheckUICageStatus.OnFeed += HandleFeeding;
        CheckUICageStatus.OnHarvest += ResetHarvestTimer;
        CalculateDropAmount();
    }

    protected virtual void OnDestroy()
    {
        CheckUICageStatus.OnFeed -= HandleFeeding;
        CheckUICageStatus.OnHarvest -= ResetHarvestTimer;
    }

    private void InitializeCage()
    {
        feedingTimer = digestionTime;
        timeUntilHarvest = harvestDuration;
    }

    private void Update()
    {
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        feedingTimer -= Time.deltaTime;
        timeUntilHarvest -= Time.deltaTime;


        if (feedingTimer <= 0) SetState(CageState.Hungry);
        if (timeUntilHarvest <= 0)
        {
            SetState(CageState.TakeProduce);
        }
    }



    private void SetState(CageState newState)
    {
        state = newState;
    }

    private void CalculateDropAmount()
    {
        dropAmount = UnityEngine.Random.Range(3, 5);
        buffDropRate = UnityEngine.Random.Range(1, 3);
        debuffDropRate = UnityEngine.Random.Range(1, 2);
    }



    public virtual void HandleFeeding(int nutrition)
    {
        if (nutrition <= 0)
        {
            Debug.LogWarning("Invalid nutrition value.");
            return;
        }

        timeUntilHarvest -= nutrition;
        feedingTimer = digestionTime;
    }

    public virtual void ResetHarvestTimer()
    {
        CalculateDropAmount();
        Debug.Log("Reset harvest timer.");
        HarvestItem();

        // Reset lại thời gian để không bị auto TakeProduce
        timeUntilHarvest = harvestDuration;
    }


    public virtual void HarvestItem()
    {
        CalculateDropAmount();
        Debug.Log("Item picked up.");
        EventBus.Publish(new ItemPickedUp(itemDrop.itemName, dropAmount));
    }
}
