using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryDisplay : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform cropContainersParent;
    [SerializeField] private Transform materialContainerParent;
    [SerializeField] private Transform toolContainersParent;
    [SerializeField] private UIcropContainer uicropContainer;

    public void Configure(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();
        foreach (var item in items)
        {
            Transform parent = GetParentByItemType(item.itemType);  // Lấy parent dựa trên ItemType
            UIcropContainer containerInstance = Instantiate(uicropContainer, parent);
            Sprite itemIcon = DataManagers.instance.GetItemSpriteFromName(item.itemName);
            containerInstance.Configure(itemIcon, item.amount, item.itemName);
        }
    }

    public void UpdateDisplay(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        // Xóa các item không còn trong kho
        RemoveItemIconFromInventory(inventory);

        // Cập nhật lại danh sách item trong UI
        foreach (var item in items)
        {
            Transform parent = GetParentByItemType(item.itemType);
            bool itemExists = false;

            // Kiểm tra nếu item đã tồn tại trong UI
            foreach (Transform child in parent)
            {
                UIcropContainer container = child.GetComponent<UIcropContainer>();
                if (container != null && container.GetIcon() == DataManagers.instance.GetItemSpriteFromName(item.itemName))
                {
                    // Nếu item đã tồn tại, cập nhật số lượng mới
                    itemExists = true;
                    container.UpdateAmount(item.amount);  // Thêm dòng này để cập nhật số lượng
                    break;
                }
            }

            // Nếu item chưa có trong UI, tạo mới
            if (!itemExists)
            {
                UIcropContainer containerInstance = Instantiate(uicropContainer, parent);
                Sprite itemIcon = DataManagers.instance.GetItemSpriteFromName(item.itemName);
                containerInstance.Configure(itemIcon, item.amount,item.itemName);
            }
        }
    }




    private void RemoveItemIconFromInventory(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        // Kiểm tra và xóa các item có số lượng = 0
        foreach (Transform parent in new Transform[] { cropContainersParent, materialContainerParent, toolContainersParent })
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                UIcropContainer container = parent.GetChild(i).GetComponent<UIcropContainer>();

                if (container == null) continue;

                string itemName = DataManagers.instance.GetItemNameFromSprite(container.GetIcon());
                InventoryItem item = System.Array.Find(items, x => x.itemName == itemName);

                if (item == null || item.amount <= 0)
                {
                    Destroy(container.gameObject); // Xóa container này
                }
            }
        }
    }


    private Transform GetParentByItemType(ItemType itemType)
    {
        // Trả về container cha tương ứng với loại món đồ
        switch (itemType)
        {
            case ItemType.Seed:
                return cropContainersParent;
            case ItemType.Produce:
                return cropContainersParent;
            case ItemType.Material:
                return materialContainerParent;
            case ItemType.Tool:
                return toolContainersParent;
            default:
                return cropContainersParent; // Mặc định
        }
    }

    

}
