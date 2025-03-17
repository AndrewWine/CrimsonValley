using System.Collections.Generic;
using UnityEngine;
using System;

public class TraderInteraction : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private GameObject WindowPanel;

    [Header("Actions")]
    public static Action EnableMarketWindow;
    public static Action<bool> OpenedMarketWindow;

    private void Start()
    {
        TradeWindow.SellItem += OnSellItem; // Đăng ký sự kiện bán hàng
        TradeWindow.BuyItem += OnBuyItem; // Đăng ký sự kiện mua hàng
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        WindowPanel.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        TradeWindow.SellItem -= OnSellItem; // Hủy đăng ký sự kiện khi đối tượng bị hủy
        TradeWindow.BuyItem -= OnBuyItem;
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
        Debug.Log("Buy item trade");

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager chưa được gán!");
            return;
        }

        Inventory inventory = inventoryManager.GetInventory();
        int coinsSpent = 0;

        foreach (var boughtEntry in boughtItems)
        {
            ItemData boughtItem = boughtEntry.Key;
            int quantityToBuy = boughtEntry.Value;

            int itemPrice = DataManagers.instance.GetItemPriceFromItemName(boughtItem.itemName);
            int totalPrice = itemPrice * quantityToBuy;

            // Kiểm tra xem người chơi có đủ tiền không
            if (CashManager.instance.GetCoins() >= totalPrice)
            {
                // Trừ tiền
                coinsSpent += totalPrice;
                CashManager.instance.SpendCoins(totalPrice);

                // Thêm vật phẩm vào kho
                inventory.AddItemByName(boughtItem.itemName, quantityToBuy);

                Debug.Log($"Mua {boughtItem.itemName} x{quantityToBuy} với giá {totalPrice} coins.");
            }
            else
            {
                Debug.LogError($"Không đủ tiền để mua {boughtItem.itemName} x{quantityToBuy}!");
            }
        }

        Debug.Log($"Đã tiêu {coinsSpent} coins để mua vật phẩm.");
        inventoryManager.GetInventoryDisplay().UpdateDisplay(inventory);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Detected - Mở bảng giao dịch!");

            if (WindowPanel == null)
            {
                Debug.LogError("WindowPanel chưa được gán! Không thể mở cửa sổ.");
                return;
            }

            EnableMarketWindow?.Invoke();
            OpenedMarketWindow?.Invoke(true);
            WindowPanel.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Rời Khỏi - Đóng bảng giao dịch!");
            WindowPanel.gameObject.SetActive(false);
        }
    }

    public void CloseMarketWindow()
    {
        TooltipManager.Instance.HideTooltip();
        WindowPanel.gameObject.SetActive(false);
        OpenedMarketWindow?.Invoke(false);

    }


}
