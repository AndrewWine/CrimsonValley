using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIcropContainer : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    [Header("Elements")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;

    private string itemName; // Lưu tên vật phẩm
    private ItemType itemType; // Lưu loại item
    private EquipType equipType; // Lưu loại item

    private static UIcropContainer currentItem; // Biến để lưu item đang hiển thị tooltip

    public void Configure(Sprite icon, int amount, string name)
    {
        iconImage.sprite = icon;
        amountText.text = amount.ToString();
        itemName = name;
        itemType = DataManagers.instance.GetItemTypeFromItemName(itemName);
        equipType = DataManagers.instance.GetEquipTypeFromItemName(itemName);

    }

    public void UpdateDisplay(int amount)
    {
        amountText.text = amount.ToString();
    }

    public Sprite GetIcon()
    {
        return iconImage.sprite;
    }

    public int GetItemPrice()
    {
        return DataManagers.instance.GetItemPriceFromItemName(itemName);
    }

    public ItemType GetItemType()
    {
        return itemType;

    }

    public EquipType GetEquipType()
    {
        return equipType;
    }

    public int GetItemDamage()
    {
        return DataManagers.instance.GetItemDamageFromItemName(itemName);
    }

    public int GetItemDurability()
    {
        return DataManagers.instance.GetItemDurabilityFromItemName(itemName);
    }

    public void UpdateAmount(int newAmount)
    {
        amountText.text = newAmount.ToString();
    }

    // Khi chạm vào item trong Inventory
    public void OnPointerClick(PointerEventData eventData)
    {
        // Kiểm tra nếu tooltip đang hiển thị, ẩn nó đi
        if (currentItem != null && currentItem != this)
        {
            TooltipManager.Instance.HideTooltip(); // Ẩn tooltip của item trước đó
        }

        // Lấy các giá trị cần thiết từ item
        int price = GetItemPrice();
        int damage = GetItemDamage();
        int durability = GetItemDurability();
        Sprite icon = GetIcon();
        string itemName = this.itemName;
        EquipType equipType = GetEquipType();
        // Tạo ItemData bằng cách sử dụng ScriptableObject.CreateInstance
        ItemData itemData = ScriptableObject.CreateInstance<ItemData>(); // Tạo instance từ ScriptableObject

        // Cập nhật thông tin vào itemData
        itemData.itemName = itemName;
        itemData.icon = icon;
        itemData.price = price;
        itemData.damage = damage;
        itemData.durability = durability;
        itemData.equipType = equipType;
        // Gọi hàm để hiển thị tooltip
        TooltipManager.Instance.ShowToolTipOnTradeWindow(itemData);

        // Lưu item hiện tại đang hiển thị tooltip
        currentItem = this;
    }


    // Khi rời khỏi item
    public void OnPointerExit(PointerEventData eventData)
    {
        // Tooltip sẽ không bị ẩn nếu item hiện tại đang được chọn
        if (currentItem != this)
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}
