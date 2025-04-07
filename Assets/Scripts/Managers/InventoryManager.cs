using System.IO;
using UnityEngine;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private Inventory inventory;
    private InventoryDisplay inventoryDisplay;
    private string dataPath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        dataPath = Application.persistentDataPath + "/inventoryData.txt";
        LoadInventory();
        ConfigureInventoryDisplay();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<ItemPickedUp>(OnItemPickedUpEvent);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ItemPickedUp>(OnItemPickedUpEvent);

    }

    private void OnItemPickedUpEvent(ItemPickedUp eventData)
    {
        PickUpItemCallBack(eventData.itemName, eventData.amount);

    }
    public void PickUpItemCallBack(string itemName, int amount)
    {
        //Debug.Log($"Nhận vật phẩm: {itemName} (x{amount})");
        EventBus.Publish(new ShowItemPickup(itemName, amount));

        if (inventory == null)
        {
            Debug.LogError("Inventory chưa được khởi tạo!");
            return;
        }

        inventory.AddItemByName(itemName, amount);
        SaveInventory();

        if (inventoryDisplay != null)
        {
            inventoryDisplay.UpdateDisplay(inventory);
        }
        else
        {
            Debug.LogWarning("inventoryDisplay chưa được khởi tạo!");
        }
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

    public void LoadInventory()
    {
        if (File.Exists(dataPath))
        {
            string data = File.ReadAllText(dataPath);
            Inventory tempInventory = JsonUtility.FromJson<Inventory>(data);

            if (tempInventory != null)
            {
                inventory = tempInventory;
            }
            else
            {
                inventory = new Inventory();
                Debug.LogWarning("Dữ liệu Inventory bị lỗi, khởi tạo Inventory mới.");
            }
        }
        else
        {
            File.Create(dataPath).Close();
            inventory = new Inventory();
        }
    }


    public bool FindItemByName(string itemName, int requiredAmount)
    {
        if (inventory == null) return false;

        InventoryItem item = inventory.FindItem(itemName);
        if (item == null) return false;

        return item.amount >= requiredAmount;
    }


    public void SaveInventory()
    {
        string data = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(dataPath, data);
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    public InventoryDisplay GetInventoryDisplay()
    {
        return inventoryDisplay;
    }
}
