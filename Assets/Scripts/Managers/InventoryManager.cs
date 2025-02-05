using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{


    private Inventory inventory;
    private InventoryDisplay inventoryDisplay;
    private string dataPath;

    private void Start()
    {
        dataPath = Application.dataPath + "/inventoryData.txt";
        //inventory = new Inventory();
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



    private void PickUpItemCallBack(ItemType itemType)
    {
        //Update inventory
        inventory.CropHarvestedCallback(itemType);

        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();
    }

    private void ConfigureInventoryDisplay()
    {
        inventoryDisplay = GetComponent<InventoryDisplay>();
        inventoryDisplay.Configure(inventory);
    }

    [NaughtyAttributes.Button]
    public void ClearInventory()
    {
        inventory.Clear();
        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();
    }

    private void LoadInventory()
    {
        string data = "";
        if(File.Exists(dataPath))
        {
            data = File.ReadAllText(dataPath);
            inventory = JsonUtility.FromJson<Inventory>(data);
            if(inventory == null )
                inventory = new Inventory();
        }
        else
        {
            File.Create(dataPath);
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
