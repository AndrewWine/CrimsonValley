using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BlackSmithInteraction : UIRequirementDisplay
{
    [Header("Elements")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private GameObject WindowShopPanel;
    [SerializeField] private GameObject WindowCraftingPanel;    



    [Header("Actions")]
    public static Action EnableSmithyWindow;
    public static Action generateItemRequire;
    public static Action<bool> OpenedSmithyWindow;
    private void Start()
    {
        InitializeSettings();
    }
    private void OnEnable()
    {
        BlackSmithWindow.BuyItem += OnBuyItem;
        BlackSmithWindow.SellItem += OnSellItem;
    }

    private void InitializeSettings()
    {
        WindowShopPanel.gameObject.SetActive(false);
        WindowCraftingPanel.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        BlackSmithWindow.BuyItem -= OnBuyItem;
        BlackSmithWindow.SellItem -= OnSellItem;

    }
    private void OnSellItem(Dictionary<ItemData, int> soldItems)
    {

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager chưa được gán!");
            return;
        }

        Inventory inventory = inventoryManager.GetInventory();
        int coinsEarned = 0;

        foreach (var soldEntry in soldItems)
        {
            ItemData soldItem = soldEntry.Key;
            int quantityToSell = soldEntry.Value;

            InventoryItem[] items = inventory.GetInventoryItems();

            // Tìm vật phẩm trong kho để bán
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].itemName == soldItem.itemName)
                {
                    int itemPrice = DataManagers.instance.GetItemPriceFromItemName(items[i].itemName);

                    // Kiểm tra số lượng vật phẩm trong kho
                    if (items[i].amount >= quantityToSell)
                    {
                        // Thực hiện bán vật phẩm
                        coinsEarned += itemPrice * quantityToSell;
                        inventory.RemoveItemByName(items[i].itemName, quantityToSell);
                    }
                    else
                    {
                        Debug.LogError($"Không đủ {soldItem.itemName} để bán! (Yêu cầu: {quantityToSell}, Hiện có: {items[i].amount})");
                    }
                    break;
                }
            }
        }

        Debug.Log($"Đã kiếm được {coinsEarned} coins.");
        CashManager.instance.AddCoins(coinsEarned);

        inventoryManager.GetInventoryDisplay().UpdateDisplay(inventory);
    }

    private void OnBuyItem(Dictionary<ItemData, int> boughtItems)
    {
        Debug.Log("Buy item smithy");
        if (inventoryManager == null)
        {
            Debug.LogError(" InventoryManager chưa được gán!");
            return;
        }

        int coinsSpent = 0;

        foreach (var boughtEntry in boughtItems)
        {
            ItemData boughtItem = boughtEntry.Key;
           

            int itemPrice = DataManagers.instance.GetItemPriceFromItemName(boughtItem.itemName);
            int totalPrice = itemPrice * boughtItems.Count;

            // Kiểm tra xem người chơi có đủ tiền không
            if (CashManager.instance.GetCoins() >= totalPrice)
            {
                // Trừ tiền
                coinsSpent += totalPrice;
                CashManager.instance.SpendCoins(totalPrice);

                // Thêm vào kho
                inventoryManager.PickUpItemCallBack(boughtItem.itemName, boughtItems.Count);

                Debug.Log($" Mua {boughtItem.itemName} x{boughtItems.Count} với giá {totalPrice} coins.");
            }
            else
            {
                Debug.LogError($" Không đủ tiền để mua {boughtItem.itemName} x{boughtItems.Count}!");
            }
        }

        Debug.Log($" Đã tiêu {coinsSpent} coins để mua vật phẩm.");
        inventoryManager.GetInventoryDisplay().UpdateDisplay(inventoryManager.GetInventory());
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Detected - Mở bảng giao dịch!");

            if (WindowShopPanel == null)
            {
                Debug.LogError("WindowPanel chưa được gán! Không thể mở cửa sổ.");
                return;
            }

            EnableSmithyWindow?.Invoke();
            generateItemRequire?.Invoke();//UISelectedButton
            OpenedSmithyWindow?.Invoke(true);
            Debug.Log("Mở ShopWindow");
            WindowShopPanel.gameObject.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(" Player Rời Khỏi - Đóng bảng giao dịch!");
            WindowShopPanel.gameObject.SetActive(false);
        }
    }

    public void CloseMarketWindow()
    {
        TooltipManager.Instance.HideTooltip();
        WindowShopPanel.gameObject.SetActive(false);
        OpenedSmithyWindow?.Invoke(false);
    }

    public void OnButtonCraftPressed()
    {
        WindowCraftingPanel.gameObject.SetActive(true);
        WindowShopPanel.gameObject.gameObject.SetActive(false);
    }

    public void OnShopCraftButtonPressed()
    {
        WindowShopPanel.gameObject.gameObject.SetActive(true);
        WindowCraftingPanel.gameObject.SetActive(false);
    }


}
