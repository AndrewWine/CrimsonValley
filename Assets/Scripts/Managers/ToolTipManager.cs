using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    [Header("Actions")]
    public static TooltipManager Instance;
    public static Action<ItemData> NotifyEquipItem;

    [Header("Elements")]
    private ItemData currentItemData; // Sử dụng ItemData thay vì int để lưu trữ đầy đủ thông tin item
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private TextMeshProUGUI itemDamageText;
    [SerializeField] private TextMeshProUGUI itemDurabilityText;
    [SerializeField] private Image itemIcon;
    [SerializeField] GameObject EquipButton;

    private void Awake()
    {
        Instance = this;
        tooltipPanel.SetActive(false);
    }

    private void OnEnable()
    {
        BlackSmithInteraction.OpenedSmithyWindow += CheckCanEnableEquipButton;
        TraderInteraction.OpenedMarketWindow += CheckCanEnableEquipButton;
    }
    public void ShowToolTipOnTradeWindow(ItemData itemData)
    {
        currentItemData = itemData; // Lưu ItemData đầy đủ

        itemNameText.text = itemData.itemName;
        itemIcon.sprite = itemData.icon;
        itemPriceText.text = $"Price: {itemData.price.ToString()}$";

        itemDamageText.text = itemData.damage > 0 ? $"Damage: {itemData.damage}" : "";
        itemDurabilityText.text = itemData.durability > 0 ? $"Durability: {itemData.durability}" : "";
        tooltipPanel.SetActive(true);
    }


    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
        itemNameText.text = "";
        itemPriceText.text = "";
        itemDamageText.text = "";
        itemDurabilityText.text = "";
    }

    private void CheckCanEnableEquipButton(bool check)
    {
        EquipButton.gameObject.SetActive(!check);
    }

    public void OnEquipItem()
    {
        if (currentItemData != null)
        {
            NotifyEquipItem?.Invoke(currentItemData);//EquipItem
            Debug.Log("EquipItem equiptype " + currentItemData.equipType);
        }
        else
        {
            Debug.LogWarning("ItemData is null! Cannot equip or unequip.");
        }
    }
}
