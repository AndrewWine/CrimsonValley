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
            int itemPrice = DataManagers.instance.GetItemPriceFromItemName(items[i].itemName);
            coinsEarned += itemPrice * items[i].amount;
        }

        Debug.Log("Chúng ta đã kiếm được " + coinsEarned + " coins");
        CashManager.instance.AddCoins(coinsEarned);

        inventoryManager.ClearInventory();

        // Cập nhật UI ngay sau khi bán
        inventoryManager.GetInventoryDisplay().UpdateDisplay(inventoryManager.GetInventory());

    }


}
