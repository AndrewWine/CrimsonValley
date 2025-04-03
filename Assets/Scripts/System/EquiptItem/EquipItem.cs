using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItem : MonoBehaviour
{
    private PlayerStatusManager playerStatusManager;
    private ListEquipment listEquipment;
    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] private GameObject placeAxeTool;
    [SerializeField] private GameObject placePickaxeTool;

    private void Start()
    {
        playerStatusManager = GetComponent<PlayerStatusManager>();
        listEquipment = GetComponent<ListEquipment>();
        TooltipManager.NotifyEquipItem += Equip;
        Tree.reduceDurability += ReduceAxeDurability;
        OreRock.decreasedPickAxeDurability += ReduceAxeDurability;

    }

    private void OnDisable()
    {
        TooltipManager.NotifyEquipItem -= Equip;
        Tree.reduceDurability -= ReduceAxeDurability;
        OreRock.decreasedPickAxeDurability -= ReduceAxeDurability;

    }
    public void Equip(ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogError("[EquipItem] ItemData không hợp lệ.");
            return;
        }

        // Kiểm tra xem đã có item cùng loại đang được trang bị không
        ItemData equippedItem = listEquipment.GetEquippedItemByEquipType(itemData.equipType);
        if (equippedItem != null)
        {
            Debug.Log($"[EquipItem] {equippedItem.itemName} đã được trang bị, tháo ra để thay thế bằng {itemData.itemName}.");
            UnEquip(equippedItem);
        }

        // Thêm item mới vào danh sách trang bị
        listEquipment.AddItem(itemData);

        // Cập nhật damage vào Blackboard dựa trên loại item
        switch (itemData.equipType)
        {
            case EquipType.Axe:
                ApplyAxeStatsToBlackboard(itemData.damage, true);
                break;

            case EquipType.Pickaxe:
                ApplyPickaxeStatsToBlackboard(itemData.damage, true);
                break;

            default:
                Debug.LogWarning($"[EquipItem] Item {itemData.itemName} có equipType không xác định.");
                break;
        }

        // Hiển thị icon tại đúng vị trí
        ShowItemIcon(itemData);

        Debug.Log($"[EquipItem] {itemData.itemName} đã được trang bị.");
    }

    public void UnEquip(ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogError("[UnEquip] ItemData không hợp lệ.");
            return;
        }

        // Xóa item khỏi danh sách trang bị
        listEquipment.RemoveItem(itemData);

        // Giảm chỉ số tương ứng
        switch (itemData.equipType)
        {
            case EquipType.Axe:
                ApplyAxeStatsToBlackboard(itemData.damage, false);
                break;

            case EquipType.Pickaxe:
                ApplyPickaxeStatsToBlackboard(itemData.damage, false);
                break;

            default:
                Debug.LogWarning($"[UnEquip] Item {itemData.itemName} có equipType không xác định.");
                break;
        }

        // Ẩn icon khi tháo trang bị
        HideItemIcon(itemData);

        Debug.Log($"[UnEquip] {itemData.itemName} đã được tháo.");
    }

    private void ShowItemIcon(ItemData itemData)
    {
        if (itemData == null) return;

        Image itemImage = null;
        if (itemData.equipType == EquipType.Axe && placeAxeTool != null)
            itemImage = placeAxeTool.GetComponent<Image>();
        else if (itemData.equipType == EquipType.Pickaxe && placePickaxeTool != null)
            itemImage = placePickaxeTool.GetComponent<Image>();

        if (itemImage != null)
        {
            itemImage.sprite = itemData.icon;
            itemImage.SetNativeSize();
            RectTransform rectTransform = itemImage.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(100, 100);
            }
        }
    }

    private void HideItemIcon(ItemData itemData)
    {
        if (itemData == null) return;

        Image itemImage = null;
        if (itemData.equipType == EquipType.Axe && placeAxeTool != null)
            itemImage = placeAxeTool.GetComponent<Image>();
        else if (itemData.equipType == EquipType.Pickaxe && placePickaxeTool != null)
            itemImage = placePickaxeTool.GetComponent<Image>();

        if (itemImage != null)
        {
            itemImage.sprite = null;
        }
    }

    private void ApplyAxeStatsToBlackboard(int itemDamage, bool isEquip)
    {
        if (itemDamage > 0)
        {
            if (isEquip)
                playerStatusManager.AddAxeDamage(itemDamage);
            else
                playerStatusManager.RemoveAxeDamage(itemDamage);
        }
    }

    private void ApplyPickaxeStatsToBlackboard(int itemDamage, bool isEquip)
    {
        if (itemDamage > 0)
        {
            if (isEquip)
                playerStatusManager.AddPickaxeDamage(itemDamage);
            else
                playerStatusManager.RemovePickaxeDamage(itemDamage);
        }
    }

    public void ReducePickaxeDurability()
    {
        ItemData pickaxeEquip = ListEquipment.Instance?.GetEquippedItemByEquipType(EquipType.Pickaxe);

        if (pickaxeEquip != null && pickaxeEquip.equipType == EquipType.Pickaxe)
        {
            if (pickaxeEquip.durability > 0)
            {
                pickaxeEquip.durability -= 1;
                Debug.LogWarning($"Durability of {pickaxeEquip.itemName} decreased to {pickaxeEquip.durability}");
            }

            if (pickaxeEquip.durability <= 0)
            {
                Debug.Log($"{pickaxeEquip.itemName} is broken!");

                // Gỡ trang bị
                UnEquip(pickaxeEquip);

                // Kiểm tra số lượng trước khi xóa
                Inventory inventory = InventoryManager.Instance.GetInventory();
                if (inventory != null && inventory.GetItemCountByName(pickaxeEquip.itemName) > 0)
                {
                    inventory.RemoveItemByName(pickaxeEquip.itemName, 1);

                }


            }
        }
    }

    public void ReduceAxeDurability()
    {
        ItemData axeEquip = ListEquipment.Instance?.GetEquippedItemByEquipType(EquipType.Axe);

        if (axeEquip != null && axeEquip.equipType == EquipType.Axe)
        {
            if (axeEquip.durability > 0)
            {
                axeEquip.durability -= 1;
                Debug.LogWarning($"Durability of {axeEquip.itemName} decreased to {axeEquip.durability}");
            }

            if (axeEquip.durability <= 0)
            {
                Debug.Log($"{axeEquip.itemName} is broken!");

                // Gỡ trang bị
                UnEquip(axeEquip);

                // Kiểm tra số lượng trước khi xóa
                Inventory inventory = InventoryManager.Instance.GetInventory();
                if (inventory != null && inventory.GetItemCountByName(axeEquip.itemName) > 0)
                {
                    inventory.RemoveItemByName(axeEquip.itemName, 1);
                }

            }
        }
    }



}
