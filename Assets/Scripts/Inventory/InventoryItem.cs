using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InventoryItem
{
    public ItemType itemType;
    public int amount;

    // Constructor cho InventoryItem
    public InventoryItem(ItemType itemType, int amount)
    {
        this.itemType = itemType;
        this.amount = amount;
    }


}
