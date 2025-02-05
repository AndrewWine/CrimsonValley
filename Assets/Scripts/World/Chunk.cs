using UnityEngine;
using TMPro;
using System;
[RequireComponent(typeof(ChunkWalls))]
public class Chunk : MonoBehaviour
{
    
    [Header("Elements")]
    [SerializeField] private GameObject unlockedElements;
    [SerializeField] private GameObject lockedElements;
    [SerializeField] private TextMeshPro priceText;
    private ChunkWalls chunkWalls;

    [Header("Settings")]
    [SerializeField] private int initialPrice;
    private int currentPrice;
    public bool unlocked;
    [Header("Action")]
    public static Action onUnlocked;
    public static Action onPriceChanged;

    private void Awake()
    {
        chunkWalls =  GetComponent<ChunkWalls>();
    }

    private void Start()
    {
        currentPrice = initialPrice;
        priceText.text = currentPrice.ToString();
    }

    public void TryUnlock()
    {
        if (CashManager.instance.GetCoins() <= 0)
            return;

        onPriceChanged?.Invoke();

        currentPrice--;
        CashManager.instance.UseCoins(1);

        priceText.text = currentPrice.ToString();
        if(currentPrice <= 0)
            Unlock();
    }
    

    public void Unlock(bool triggerAction = true)
    {
        unlockedElements.SetActive(true);
        lockedElements.SetActive(false);

        unlocked = true;

        if(triggerAction)
            onUnlocked?.Invoke();
    }

    public bool IsUnlocked()
    {
        return unlocked;
    }

    public int GetInitialPrice()
    {
        return initialPrice; 
    }

    public int GetCurrentPrice()
    {
        return currentPrice;
    }

    internal void Initiallize(int LoadedPrice)
    {
        currentPrice = LoadedPrice;
        priceText.text = currentPrice.ToString();

        if(currentPrice <= 0)
            Unlock(false);
    }

    public void UpdateWalls(int configuration)
    {
        chunkWalls.Configure(configuration);
    }




}
