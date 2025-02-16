using System;
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
        Debug.Log("📋 Danh sách Inventory:");
        foreach (var item in items)
        {
            Debug.Log($"🔹 {item.itemName} - Số lượng: {item.amount}");
        }
        return items.ToArray();
    }


    public void Clear()
    {
        items.Clear();
    }

    /// 🔍 Tìm kiếm vật phẩm trong danh sách Inventory
    public InventoryItem FindItem(string itemName)
    {
        return items.Find(i => i.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
    }



    public void RemoveItemByName(string itemName, int amountToRemove)
    {
        InventoryItem existingItem = items.Find(item => item.itemName.Equals(itemName, System.StringComparison.OrdinalIgnoreCase));

        if (existingItem != null)
        {
            if (existingItem.amount > amountToRemove)
            {
                existingItem.amount -= amountToRemove;
            }
            else
            {
                items.Remove(existingItem);
            }

            Debug.Log($"Đã xóa {amountToRemove} {itemName} khỏi inventory. Số lượng còn lại: {existingItem.amount}");
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy {itemName} trong inventory để xóa.");
        }
    }
}
