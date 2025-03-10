using System;
using UnityEngine;
using UnityEngine.UI;

public class Cage : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] public CageState state;
    [SerializeField] private ItemData itemDrop;



    [Header("Settings")]
    [SerializeField] protected int nutrition;
    [SerializeField] protected int harvestTimer;
    [SerializeField] protected int slot;
    [SerializeField] protected float feedingTimer;
    [SerializeField] protected int DigestionTime = 10;
    protected float timeToHarvestProductPassed = 0;
    [SerializeField] protected int dropAmount = 10;

    [Header("QuantityItems")]
    [SerializeField] protected int quantity;
    [SerializeField] protected string nameOfMaterial;

    [Header("Actions")]
    public static Action<string, int> GiveItemToPlayer;

    [Header("Buff and Debuff of CrimsonMoon event")]
    private int BuffdropRate ;
    private int DeBuffdropRate ;

    protected virtual void start()
    {
        InitializeSetting();
        CheckUICageStatus.FeedButton += ReduceTimeProduce;
        CheckUICageStatus.TakeProduceButton += ResetTakeProduceTimer;
        CaculatedDropAmount();
    }

    protected virtual void CaculatedDropAmount()
    {
        dropAmount = UnityEngine.Random.Range(3, 5);
        BuffdropRate = UnityEngine.Random.Range(1, 3);
        DeBuffdropRate = UnityEngine.Random.Range(1, 2);
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
        CaculatedDropAmount();
        Debug.Log("Goi ham Reset");
        PickupItem();
        timeToHarvestProductPassed = harvestTimer;
    }

    public virtual void PickupItem()
    {
        CaculatedDropAmount();

        Debug.Log("Da gui tin hieu trc day");

        // Tính toán số lượng vật phẩm cần rơi (có thể thay đổi theo logic của bạn)
        Debug.Log("gui tin hieu");

        GiveItemToPlayer?.Invoke(itemDrop.name, dropAmount);//InventoryManager
        Debug.Log("Da gui tin hieu");
    }

}
