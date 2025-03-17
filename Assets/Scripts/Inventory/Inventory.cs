using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();

    public void AddItemByName(string itemName, int amount)
    {
        if (amount <= 0)
        {
            Debug.Log("Số lượng mua phải lớn hơn 0.");
            return;
        }

        ItemData itemData = DataManagers.instance.GetItemDataByName(itemName);

        if (itemData != null)
        {
            ItemType itemType = itemData.itemType;

            InventoryItem existingItem = items.Find(item => item.itemName == itemName);

            if (existingItem != null)
            {
                existingItem.amount += amount;
                Debug.Log("So luong" + existingItem.amount);
            }
            else
            {
                items.Add(new InventoryItem(itemName, amount, itemType));
            }
            Debug.LogWarning($"Đã thêm {amount} {itemName} vào inventory.");
        }
        else
        {
            Debug.LogError("Không tìm thấy item với tên: " + itemName);
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

    ///  Tìm kiếm vật phẩm trong danh sách Inventory
    public InventoryItem FindItem(string itemName)
    {
        return items.Find(i => i.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
    }

    public Sprite GetItemSprite(string itemName)
    {
        ItemData itemData = DataManagers.instance.GetItemDataByName(itemName);
        if (itemData != null)
        {
            return itemData.icon; // Giả sử ItemData có thuộc tính icon (Sprite)
        }
        else
        {
            Debug.LogError($"Không tìm thấy item: {itemName}");
            return null; // Hoặc có thể trả về một sprite mặc định nếu cần
        }
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
