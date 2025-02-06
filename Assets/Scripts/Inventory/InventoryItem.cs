[System.Serializable]
public class InventoryItem
{
    public string itemName;  // Tên item
    public int amount;       // Số lượng item
    public ItemType itemType;  // Kiểu của món đồ

    public InventoryItem(string itemName, int amount, ItemType itemType)
    {
        this.itemName = itemName;
        this.amount = amount;
        this.itemType = itemType;
    }
}
