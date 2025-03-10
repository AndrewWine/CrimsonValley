using UnityEngine;
using System;
using Unity.VisualScripting;

public class DataManagers : MonoBehaviour
{
    public static DataManagers instance;

    [Header("Data")]
    [SerializeField] private ItemData[] itemDataArray;
    [SerializeField] private BuildingData[] buildingDataArray;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public ItemData GetItemDataByName(string itemName)
    {
        foreach (var data in itemDataArray)
        {
            if (data.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                return data;
        }
        Debug.LogError(" Không tìm thấy ItemData: " + itemName);
        return null;
    }


    public ItemData GetItemPriceByName(string itemName)
    {
        foreach (var data in itemDataArray)
        {
            if (data.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                return data;
        }
        Debug.LogError(" Không tìm thấy ItemData: " + itemName);
        return null;
    }



    public BuildingData GetBuildingDataByName(string buildingName)
    {
        foreach (var data in buildingDataArray)
        {
            if (data.buildingName.Equals(buildingName, StringComparison.OrdinalIgnoreCase))
                return data;
        }
        Debug.LogError(" Không tìm thấy BuildingData: " + buildingName);
        return null;
    }

    public Sprite GetItemSpriteFromName(string name)
    {
        // Tìm trong ItemData trước
        ItemData itemData = GetItemDataByName(name);
        if (itemData != null) return itemData.icon;
        Debug.LogError($"Không tìm thấy sprite cho: {name}");

        // Nếu không tìm thấy, tìm trong BuildingData
        BuildingData buildingData = GetBuildingDataByName(name);
        if (buildingData != null) return buildingData.icon;

        Debug.LogError($"Không tìm thấy sprite cho: {name}");
        return null;
    }

    public int GetItemPriceFromItemName(string itemName)
    {
        foreach (var data in itemDataArray)
        {
            if (data.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                return data.price;
        }
        Debug.LogError("Không tìm thấy giá của: " + itemName);
        return 0;
    }

    public string GetItemNameFromSprite(Sprite sprite)
    {
        foreach (var data in itemDataArray)
        {
            if (data.icon == sprite)
                return data.itemName;
        }
        foreach (var data in buildingDataArray)
        {
            if (data.icon == sprite)
                return data.buildingName;
        }
        Debug.LogError(" Không tìm thấy tên item từ sprite.");
        return null;
    }

    public ItemType GetItemTypeFromItemName(string itemName)
    {
        foreach (var data in itemDataArray)
        {
            if (data.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
            {
                return data.itemType;  // Trả về loại item (ItemType)
            }
        }
        Debug.LogError("Không tìm thấy ItemType cho item: " + itemName);
        return ItemType.Produce;  // Hoặc trả về giá trị mặc định (tùy theo enum của bạn)
    }

    public EquipType GetEquipTypeFromItemName(string itemName)
    {
        foreach (var data in itemDataArray)
        {
            if (data.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
            {
                return data.equipType;  // Trả về loại item (EquipType)
            }
        }
        Debug.LogError("Không tìm thấy ItemType cho item: " + itemName);
        return EquipType.Pickaxe;  // Hoặc trả về giá trị mặc định (tùy theo enum của bạn)
    }

    public int GetItemDamageFromItemName(string itemName)
    {
        foreach (var data in itemDataArray)
        {
            if (data.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                return data.damage;
        }
        Debug.LogError("Không tìm thấy damage của: " + itemName);
        return 0;
    }

    public int GetItemDurabilityFromItemName(string itemName)
    {
        foreach (var data in itemDataArray)
        {
            if (data.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                return data.durability;
        }
        Debug.LogError("Không tìm thấy durability của: " + itemName);
        return 0;
    }
}
