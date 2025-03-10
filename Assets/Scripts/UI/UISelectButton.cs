using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISelectButton : MonoBehaviour
{
    [Header("Building System")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Transform buildingButtonContainer;
    [SerializeField] private BuildingData[] architectureData;
    [SerializeField] private Button buildingButtonPrefab;

    [Header("SeedUI")]
    [SerializeField] private Transform seedButtonContainer;
    [SerializeField] private Button seedButtonPrefab;

    [Header("Seeds Data")]
    [SerializeField] private List<ItemData> seedDataList;

    [Header("TradeWindowUI")]
    [SerializeField] private Transform tradeButtonContainer;
    [SerializeField] private Button tradeButtonPrefab;

    [Header("CraftWindowUI")]
    [SerializeField] private Transform craftShopButtonContainer;
    [SerializeField] private Button craftShopButtonPrefab;
    [SerializeField] private Transform craftingButtonContainer;
    [SerializeField] private Button craftingButtonPrefab;

    [Header("Item trade Data")]
    [SerializeField] private List<ItemData> itemDataList;

    [Header("Item craft Data")]
    [SerializeField] private List<ItemData> craftDataList;

    [Header("Inventory")]
    [SerializeField] private Inventory playerInventory;

    [Header("Actions")]
    public static Action<BuildingData> buildButtonPressed;
    public static Action<ItemData> seedButtonPressed;
    public static Action<ItemData> tradeButtonPressed;
    public static Action<ItemData> tradeShopCraftButtonPressed;
    public static Action<ItemData> craftButtonPressed;


    private Dictionary<ItemData, Transform> clickedBorderDictionary = new();

    private void OnEnable()
    {
        BuildingSystem.generateButton += () => GenerateButtons(architectureData, buildingButtonContainer, buildingButtonPrefab, buildButtonPressed);
        PlayerSnowAbility.generateSeedUIButton += GenerateSeedButtons;
        TraderInteraction.EnableMarketWindow += () => GenerateButtons(itemDataList, tradeButtonContainer, tradeButtonPrefab, tradeButtonPressed, true);
        BlackSmithInteraction.EnableSmithyWindow += () => GenerateButtons(craftDataList, craftShopButtonContainer, craftShopButtonPrefab, tradeShopCraftButtonPressed, true);
        BlackSmithInteraction.generateItemRequire += () => GenerateButtons(craftDataList, craftingButtonContainer, craftingButtonPrefab, craftButtonPressed, true);

    }

    private void OnDisable()
    {
        BuildingSystem.generateButton -= () => GenerateButtons(architectureData, buildingButtonContainer, buildingButtonPrefab, buildButtonPressed);
        PlayerSnowAbility.generateSeedUIButton -= GenerateSeedButtons;
        TraderInteraction.EnableMarketWindow -= () => GenerateButtons(itemDataList, tradeButtonContainer, tradeButtonPrefab, tradeButtonPressed, true);
        BlackSmithInteraction.EnableSmithyWindow -= () => GenerateButtons(craftDataList, craftShopButtonContainer, craftShopButtonPrefab, tradeShopCraftButtonPressed, true);
        BlackSmithInteraction.generateItemRequire -= () => GenerateButtons(craftDataList, craftingButtonContainer, craftingButtonPrefab, craftButtonPressed, true);


    }

    private void GenerateSeedButtons()
    {
        if (inventoryManager == null || inventoryManager.GetInventory() == null)
        {
            Debug.LogError("InventoryManager chưa được gán hoặc Inventory chưa được khởi tạo!");
            return;
        }

        Inventory inventory = inventoryManager.GetInventory();
        InventoryItem[] inventoryItems = inventory.GetInventoryItems();
        List<ItemData> validSeeds = new();

        foreach (InventoryItem invItem in inventoryItems)
        {
            ItemData itemData = DataManagers.instance.GetItemDataByName(invItem.itemName);
            if (itemData != null && itemData.itemType == ItemType.Seed)
            {
                validSeeds.Add(itemData);
            }
        }

        GenerateButtons(validSeeds, seedButtonContainer, seedButtonPrefab, seedButtonPressed);
    }

    private void GenerateButtons<T>(IEnumerable<T> dataList, Transform buttonContainer, Button buttonPrefab, Action<T> onClickAction, bool isTradeUI = false)
    {
        // Xóa nút cũ
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        if (isTradeUI) clickedBorderDictionary.Clear(); // Nếu là Trade UI, reset ClickedBorder dictionary

        foreach (var data in dataList)
        {
            Button newButton = Instantiate(buttonPrefab, buttonContainer);
            string itemName;
            Sprite icon;

            if (data is BuildingData building)
            {
                itemName = building.buildingName;
                icon = building.icon;
            }
            else if (data is ItemData item)
            {
                itemName = item.itemName;
                icon = item.icon;
            }
            else continue;

            newButton.name = itemName;

            // Gán tên vào TextMeshProUGUI
            TextMeshProUGUI itemNameText = newButton.transform.Find("ItemName")?.GetComponent<TextMeshProUGUI>();
            if (itemNameText != null) itemNameText.text = itemName;

            // Gán icon vào Image
            Image iconImage = newButton.transform.Find("Icon")?.GetComponent<Image>();
            if (iconImage != null && icon != null)
            {
                iconImage.sprite = icon;
                iconImage.SetNativeSize();
                iconImage.rectTransform.sizeDelta = new Vector2(70, 70);
            }

            // Nếu là Trade UI, thêm ClickedBorder vào Dictionary
            if (isTradeUI && data is ItemData tradeItem)
            {
                Transform clickedBorder = newButton.transform.Find("ClickedBorder");
                if (clickedBorder != null)
                {
                    clickedBorder.gameObject.SetActive(false);
                    clickedBorderDictionary[tradeItem] = clickedBorder;
                }
            }

            // Thêm sự kiện click
            newButton.onClick.AddListener(() => onClickAction?.Invoke(data));

            Debug.Log($"Tạo nút {itemName} với icon {icon?.name}");
        }
    }

    public void SetClickedBorderActive(ItemData item, bool isActive)
    {
        if (clickedBorderDictionary.TryGetValue(item, out Transform clickedBorder))
        {
            clickedBorder.gameObject.SetActive(isActive);
            Debug.Log($"🔹 ClickedBorder của {item.itemName} đã {(isActive ? "bật" : "tắt")}");
        }
        else
        {
            Debug.LogError($"Không tìm thấy ClickedBorder cho {item.itemName}");
        }
    }
}
