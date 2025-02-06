using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();

    public void AddItemByName(string itemName)
    {
        // Đảm bảo rằng itemType là kiểu ItemType, không phải int
        ItemData itemData = DataManagers.instance.GetItemDataByName(itemName);

        if (itemData != null)
        {
            ItemType itemType = itemData.itemType;

            InventoryItem existingItem = items.Find(item => item.itemName == itemName);

            if (existingItem != null)
            {
                existingItem.amount++;
            }
            else
            {
                items.Add(new InventoryItem(itemName, 1, itemType)); // Đảm bảo là itemType là ItemType
            }
        }
        else
        {
            Debug.LogError("No item found with name: " + itemName);
        }

    }



    public InventoryItem[] GetInventoryItems()
    {
        return items.ToArray();
    }

    public void Clear()
    {
        items.Clear();
    }
}
