using System;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
    [SerializeField]private Inventory inventory;
    private InventoryDisplay inventoryDisplay;
    private string dataPath;

    private void Start()
    {
        dataPath = Application.dataPath + "/inventoryData.txt";
        LoadInventory();
        ConfigureInventoryDisplay();
        CropTile.onCropHarvested += PickUpItemCallBack;
        Tree.onPickupWood += PickUpItemCallBack;
    }

    private void OnDestroy()
    {
        CropTile.onCropHarvested -= PickUpItemCallBack;
        Tree.onPickupWood -= PickUpItemCallBack;
    }

    private void PickUpItemCallBack(string itemName)
    {
        inventory.AddItemByName(itemName);
        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();
    }

    private void ConfigureInventoryDisplay()
    {
        inventoryDisplay = GetComponent<InventoryDisplay>();
        inventoryDisplay.Configure(inventory);
    }

    

    public void ClearInventory()
    {
        inventory.Clear();
        inventoryDisplay.UpdateDisplay(inventory);

        InventoryItem[] itemsAfterClear = inventory.GetInventoryItems();

        SaveInventory();
    }

    public InventoryDisplay GetInventoryDisplay()
    {
        return inventoryDisplay;
    }

    private void LoadInventory()
    {
        if (File.Exists(dataPath))
        {
            string data = File.ReadAllText(dataPath);
            Debug.Log(" Dữ liệu Inventory load từ file:\n" + data); // Log JSON để kiểm tra

            inventory = JsonUtility.FromJson<Inventory>(data);

            if (inventory == null)
            {
                Debug.LogWarning("Dữ liệu Inventory bị lỗi! Tạo Inventory mới.");
                inventory = new Inventory();
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy file Inventory! Tạo file mới.");
            File.Create(dataPath).Close();
            inventory = new Inventory();
        }

        // In ra danh sách item ngay sau khi load
        Debug.Log(" Inventory sau khi load:");
        InventoryItem[] items = inventory.GetInventoryItems();
        foreach (var item in items)
        {
            Debug.Log($"   - {item.itemName} ({item.itemType}): {item.amount}");
        }
    }


    public bool FindItemByName(string itemName, int requiredAmount)
    {
        if (inventory == null)
        {
            Debug.LogError("❌ Inventory chưa được khởi tạo!");
            return false;
        }

        InventoryItem item = inventory.FindItem(itemName);

        if (item != null)
        {
            Debug.Log($"🔍 Tìm thấy: {item.itemName} - Số lượng: {item.amount} (Cần: {requiredAmount})");

            // Kiểm tra xem có đủ số lượng không
            if (item.amount >= requiredAmount)
            {
                return true; // ✅ Đủ nguyên liệu
            }
            else
            {
                Debug.LogWarning($"⚠️ Không đủ {itemName}. Hiện có: {item.amount}, Cần: {requiredAmount}");
                return false; // ❌ Thiếu nguyên liệu
            }
        }
        else
        {
            Debug.LogWarning($"❌ Không tìm thấy {itemName} trong Inventory.");
            return false;
        }
    }



    private void SaveInventory()
    {
        string data = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(dataPath, data);
    }

    public Inventory GetInventory()
    {
        return inventory;
    }
}
