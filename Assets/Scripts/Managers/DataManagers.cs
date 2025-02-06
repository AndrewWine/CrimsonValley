using UnityEngine;
using System;
public class DataManagers : MonoBehaviour
{
    public static DataManagers instance;

    [Header("Data")]
    [SerializeField] private ItemData[] itemDataArray;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Phương thức tìm kiếm ItemData theo tên item
    public ItemData GetItemDataByName(string itemName)
    {
        foreach (var data in itemDataArray)
        {
            if (data.itemName.Equals(itemName, System.StringComparison.OrdinalIgnoreCase))
                return data;
        }
        Debug.LogError("No ItemData found for item name: " + itemName);
        return null;
    }

    public Sprite GetItemSpriteFromItemName(string itemName)
    {
        foreach (var data in itemDataArray)
        {
            if (data.itemName.Equals(itemName, System.StringComparison.OrdinalIgnoreCase))
                return data.icon;
        }
        Debug.LogError("No ItemData found for item name: " + itemName);
        return null;
    }

    public int GetItemPriceFromItemName(string itemName)
    {
        foreach (var data in itemDataArray)
        {
            if (data.itemName.Equals(itemName, System.StringComparison.OrdinalIgnoreCase))
                return data.price;
        }
        Debug.LogError("No ItemData found for item name: " + itemName);
        return 0;
    }
}
