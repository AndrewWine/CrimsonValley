using UnityEngine;

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
            Sprite itemIcon = DataManagers.instance.GetItemSpriteFromItemName(item.itemName);
            containerInstance.Configure(itemIcon, item.amount);
        }
    }

    public void UpdateDisplay(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        // Xử lý hiển thị theo từng loại ItemType
        for (int i = 0; i < items.Length; i++)
        {
            Transform parent = GetParentByItemType(items[i].itemType);  // Lấy parent dựa trên ItemType
            UIcropContainer containerInstance;

            // Kiểm tra xem container đã có sẵn chưa, nếu có thì chỉ cần cập nhật
            if (i < parent.childCount)
            {
                containerInstance = parent.GetChild(i).GetComponent<UIcropContainer>();
                containerInstance.gameObject.SetActive(true);
            }
            else
            {
                containerInstance = Instantiate(uicropContainer, parent);
            }

            Sprite itemIcon = DataManagers.instance.GetItemSpriteFromItemName(items[i].itemName);
            containerInstance.Configure(itemIcon, items[i].amount);
        }

      
    }


    private Transform GetParentByItemType(ItemType itemType)
    {
        // Trả về container cha tương ứng với loại món đồ
        switch (itemType)
        {
            case ItemType.Seed:
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
