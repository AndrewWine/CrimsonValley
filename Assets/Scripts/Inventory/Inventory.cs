using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();

    public void ItemPickedCallBack(ItemType itemType)
    {
        bool itemFound = false;
        for (int i = 0; i < items.Count; i++)
        {
            InventoryItem item = items[i];

            if (item.itemType == itemType)
            {
                item.amount++;
                itemFound = true;
                break;
            }
        }
        //DebugInventory();

        if (itemFound)
            return;

        // Create a new item in the list with that corpType
        items.Add(new InventoryItem(itemType, 1));
    }

    public void CropHarvestedCallback(ItemType itemType)
    {
        bool cropFound = false;

        for (int i = 0; i < items.Count; i++)
        {
            InventoryItem item = items[i];

            if (item.itemType == itemType)
            {
                item.amount++;
                cropFound = true;
                break;
            }
        }
        //DebugInventory();

        if (cropFound)
            return;

        // Create a new item in the list with that corpType
        items.Add(new InventoryItem(itemType, 1));
    }

    public InventoryItem[] GetInventoryItems()
    {
        return items.ToArray();
    }
    public void DebugInventory()
    {
        foreach (InventoryItem item in items)
            Debug.Log("We have " + item.amount + " items of type: " + item.itemType);
    }


    public void Clear()
    {
        items.Clear();
    }
}