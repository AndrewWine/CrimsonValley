using System;
using System.Collections;
using System.Collections.Generic;
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
        if (currentItem != null && currentItem != clickedItem)
        {
        
            selectedItems.Remove(currentItem);
            TooltipManager.Instance.ShowToolTipOnTradeWindow(currentItem);
        }

        if (selectedItems.Contains(clickedItem))
        {
            // Bỏ chọn vật phẩm
            selectedItems.Remove(clickedItem);
           
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

            // Nếu đã nhập số lượng trước đó, hiển thị lại
            quantityInputField.text = listItemChosen.ContainsKey(currentItem) ? listItemChosen[currentItem].ToString() : "";

      
            TooltipManager.Instance.ShowToolTipOnTradeWindow(currentItem);
        }
    }

    public void OnFinishButtonPressed()
    {
        if (currentItem == null) return;

        if (int.TryParse(quantityInputField.text, out int quantity) && quantity > 0)
        {
            // Cập nhật số lượng vật phẩm
            listItemChosen[currentItem] = quantity;
            Debug.Log($" {currentItem.itemName} x{quantity} đã được thêm vào danh sách.");
        }
        else
        {
            Debug.LogError(" Số lượng nhập không hợp lệ!");
            return;
        }

        enterQuantity.SetActive(false);
    }

    private void ResetTradeSelection()
    {
    
        selectedItems.Clear();
        enterQuantity.SetActive(false);
    }

    public void OnSellButtonPressed()
    {
        if (listItemChosen.Count > 0)
        {
            Debug.Log(" Danh sách vật phẩm bán:");
            foreach (var item in listItemChosen)
            {
                Debug.Log($"{item.Key.itemName} x{item.Value}");
            }

            SellItem?.Invoke(new Dictionary<ItemData, int>(listItemChosen));
            listItemChosen.Clear(); // Chỉ xóa sau khi bán thành công
            ResetTradeSelection();
        }
        else
        {
            Debug.Log(" Không có vật phẩm nào để bán!");
        }
    }

    public void OnBuyButtonPressed()
    {
        if (listItemChosen.Count > 0)
        {
            BuyItem?.Invoke(new Dictionary<ItemData, int>(listItemChosen));
            ResetTradeSelection();
            listItemChosen.Clear();

        }
        else
        {
            Debug.Log("Không có vật phẩm nào để mua!");
        }
    }



    public void SelectCraftItem(ItemData itemData)
    {
        currentItemSelected = itemData;

        if (itemIconImage != null && itemData.icon != null)
        {
            itemIconImage.sprite = itemData.icon;
            itemIconImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError(" ItemData hoặc Image chưa được gán đúng!");
        }

        ShowRequiredItems(
            itemData.requiredItems,
            itemData.requiredAmounts,
            DataManagers.instance.GetItemSpriteFromName
        );

        Debug.Log($" Đã chọn {itemData.itemName}");
    }
}
