using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; } // Singleton Instance

    [SerializeField] private Inventory inventory;
    private InventoryDisplay inventoryDisplay;
    private string dataPath;

    private void Awake()
    {
        CropTile.onCropHarvested += PickUpItemCallBack;

        // Thiết lập Singleton: Nếu đã có Instance rồi thì hủy object này.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        dataPath = Application.dataPath + "/inventoryData.txt";
        LoadInventory();
        ConfigureInventoryDisplay();
    }

    private void OnEnable()
    {
        // Đăng ký sự kiện khi Object được bật



        if (Cage.GiveItemToPlayer != null)
            Cage.GiveItemToPlayer += PickUpItemCallBack;
    }

    private void OnDisable()
    {
        // Hủy đăng ký sự kiện khi Object bị tắt
        CropTile.onCropHarvested -= PickUpItemCallBack;
        Cage.GiveItemToPlayer -= PickUpItemCallBack;
    }

    public void PickUpItemCallBack(string itemName, int amount)
    {
        Debug.Log("Buy on ItemCallBack");
        if (inventory != null && inventoryDisplay != null)
        {
            inventory.AddItemByName(itemName, amount);
            SaveInventory();
            Debug.Log($"Đã thêm item {itemName} số lượng {amount}");
        }
        else
        {
            Debug.LogError("inventory hoặc inventoryDisplay là null!");
        }
        inventoryDisplay.UpdateDisplay(inventory);

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
            Debug.Log("Dữ liệu Inventory load từ file:\n" + data);

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
            Debug.LogError("Inventory chưa được khởi tạo!");
            return false;
        }

        InventoryItem item = inventory.FindItem(itemName);
        if (item != null)
        {
            Debug.Log($"Tìm thấy: {item.itemName} - Số lượng: {item.amount} (Cần: {requiredAmount})");

            if (item.amount >= requiredAmount)
            {
                return true;
            }
            else
            {
                Debug.LogWarning($" Không đủ {itemName}. Hiện có: {item.amount}, Cần: {requiredAmount}");
                return false;
            }
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy {itemName} trong Inventory.");
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
