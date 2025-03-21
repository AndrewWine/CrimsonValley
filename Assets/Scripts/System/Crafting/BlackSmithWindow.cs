﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BlackSmithWindow : UIRequirementDisplay
{
    [Header("Elements")]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject enterQuantity;
    [SerializeField] private InputField quantityInputField; // Ô nhập số lượng
    [SerializeField] private Button finishButton; // Nút hoàn tất nhập số lượng
    [SerializeField] private UISelectButton uiSelectButton;
    [SerializeField] private Image itemIconImage; // Gán Image trong Inspector


    private ItemData currentItemSelected;
    [Header("Data")]
    [SerializeField] private ItemData[] items;
    private Dictionary<ItemData, int> listItemChosen = new Dictionary<ItemData, int>(); // Lưu item + số lượng
    private HashSet<ItemData> selectedItems = new HashSet<ItemData>(); // Lưu trạng thái đã chọn

    [Header("Actions")]
    public static Action<Dictionary<ItemData, int>> SellItem;
    public static Action<Dictionary<ItemData, int>> BuyItem;

    private ItemData currentItem; // Item đang nhập số lượng
    private void Start()
    {
        enterQuantity.gameObject.SetActive(false);

        // Trước khi thêm sự kiện, xóa hết để tránh bị trùng lặp
        finishButton.onClick.RemoveAllListeners();
        finishButton.onClick.AddListener(OnFinishButtonPressed);
    }

    private void OnEnable()
    {
        UISelectButton.craftButtonPressed += SelectCraftItem;
        UISelectButton.tradeShopCraftButtonPressed += OnItemClicked;
        

    }

    private void OnDisable()
    {
        UISelectButton.craftButtonPressed -= SelectCraftItem;
        UISelectButton.tradeShopCraftButtonPressed -= OnItemClicked;

    }


    public void OnItemClicked(ItemData clickedItem)
    {

        // Nếu đang chọn một vật phẩm khác, bỏ chọn vật phẩm cũ
        if (currentItem != null && currentItem != clickedItem)
        {
            uiSelectButton.SetClickedBorderActive(currentItem, false);
            selectedItems.Remove(currentItem);
            TooltipManager.Instance.ShowToolTipOnTradeWindow(currentItem);
        }

        if (selectedItems.Contains(clickedItem))
        {
            // Nếu click vào chính nó => Bỏ chọn
            selectedItems.Remove(clickedItem);
            listItemChosen.Remove(clickedItem);
            uiSelectButton.SetClickedBorderActive(clickedItem, false);
            enterQuantity.SetActive(false);
            currentItem = null; // Reset vật phẩm hiện tại
            TooltipManager.Instance.HideTooltip();

        }
        else
        {
            // Chọn vật phẩm mới
            currentItem = clickedItem;
            selectedItems.Add(clickedItem);
            enterQuantity.SetActive(true);
            quantityInputField.text = ""; // Reset ô nhập
            uiSelectButton.SetClickedBorderActive(clickedItem, true);
            TooltipManager.Instance.ShowToolTipOnTradeWindow(currentItem);
        }
    }

    // Khi người dùng nhấn "add" sau khi nhập số lượng
    public void OnFinishButtonPressed()
    {
        if (currentItem == null) return;

        if (int.TryParse(quantityInputField.text, out int quantity) && quantity > 0)
        {
            // Thêm hoặc cập nhật số lượng item đã chọn
            listItemChosen[currentItem] = quantity;
            Debug.Log($"{currentItem.itemName} x{quantity} đã được thêm vào danh sách.");
        }
        else
        {
            Debug.LogError("Số lượng nhập không hợp lệ!");
            return;
        }

        enterQuantity.SetActive(false);
        currentItem = null; // Reset item đang nhập
    }

    // Xóa trạng thái chọn sau khi bán/mua
    private void ResetTradeSelection()
    {
        foreach (var item in selectedItems)
        {
            uiSelectButton.SetClickedBorderActive(item, false);
        }
        selectedItems.Clear();
        listItemChosen.Clear();
        enterQuantity.SetActive(false);
    }


    public void OnSellButtonPressed()
    {
        if (listItemChosen.Count > 0)
        {
            SellItem?.Invoke(new Dictionary<ItemData, int>(listItemChosen));//tradeinteraction
            ResetTradeSelection();
        }
        else
        {
            Debug.Log("Không có vật phẩm nào để bán!");
        }
    } 

    public void OnBuyButtonPressed()
    {
        if (listItemChosen.Count > 0)
        {
            BuyItem?.Invoke(new Dictionary<ItemData, int>(listItemChosen));
            ResetTradeSelection();
        }
        else
        {
            Debug.Log("Không có vật phẩm nào để mua!");
        }
    }




    public void SelectCraftItem(ItemData itemData)
    {
        currentItemSelected = itemData;

        // Kiểm tra itemData có icon không
        if (itemIconImage != null && itemData.icon != null)
        {
            itemIconImage.sprite = itemData.icon; // Gán icon vào Image UI
            itemIconImage.gameObject.SetActive(true); // Bật Image lên nếu nó đang ẩn
        }
        else
        {
            Debug.LogError("ItemData hoặc Image chưa được gán đúng!");
        }

        // Hiển thị nguyên liệu yêu cầu
        ShowRequiredItems(
            itemData.requiredItems,
            itemData.requiredAmounts,
            DataManagers.instance.GetItemSpriteFromName
        );

        Debug.Log($"Đã chọn {itemData.itemName}");
    }


}
