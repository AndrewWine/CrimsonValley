using System.IO;
using UnityEngine;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
    private Inventory inventory;
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
            inventory = JsonUtility.FromJson<Inventory>(data);
            if (inventory == null)
                inventory = new Inventory();
        }
        else
        {
            File.Create(dataPath).Close();
            inventory = new Inventory();
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
