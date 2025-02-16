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
            containerInstance.Configure(itemIcon, item.amount);
        }
    }

    public void UpdateDisplay(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        if (items.Length == 0)
        {
            HideAllItemContainers();
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            Transform parent = GetParentByItemType(items[i].itemType);
            UIcropContainer containerInstance;

            if (i < parent.childCount)
            {
                containerInstance = parent.GetChild(i).GetComponent<UIcropContainer>();
                containerInstance.gameObject.SetActive(true);
            }
            else
            {
                containerInstance = Instantiate(uicropContainer, parent);
            }

            Sprite itemIcon = DataManagers.instance.GetItemSpriteFromName(items[i].itemName); 
            containerInstance.Configure(itemIcon, items[i].amount);
        }

        RemoveItemIconFromInventory(inventory);
    }



    // Hàm ẩn tất cả item trong UI inventory
    private void HideAllItemContainers()
    {
        foreach (Transform child in cropContainersParent)
            child.gameObject.SetActive(false);

        foreach (Transform child in materialContainerParent)
            child.gameObject.SetActive(false);

        foreach (Transform child in toolContainersParent)
            child.gameObject.SetActive(false);

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
                    Destroy(container.gameObject);
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
            case ItemType.Material:
                return materialContainerParent;
            case ItemType.Tool:
                return toolContainersParent;
            default:
                return cropContainersParent; // Mặc định
        }
    }

    

}
