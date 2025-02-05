using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuyerInteractor : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private InventoryManager inventoryManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Buyer"))
            SellCrops();
    }
    
 
    private void SellCrops()
    {
        Inventory inventory = inventoryManager.GetInventory();
        InventoryItem[] items = inventory.GetInventoryItems();
        int coinsEarned = 0;
        for (int i = 0; i < items.Length; i++)
        {
            // calculate the earnings
            int itemPrice = DataManagers.instance.GetCropPriceFromCropType(items[i].itemType);
            coinsEarned += itemPrice * items[i].amount;
        }

        //give the coins to the player
        Debug.Log("we've earned" + coinsEarned + " coins");
        CashManager.instance.AddCoins(coinsEarned);
        inventoryManager.ClearInventory();
    }
}
