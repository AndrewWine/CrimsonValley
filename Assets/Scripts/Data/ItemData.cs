using UnityEngine;



[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("Settings")]
    public string itemName; // Tên item
    public ItemType itemType; // ItemType có thể mang nhiều giá trị
    public EquipType equipType; 
    public Item itemPrefab;
    public Sprite icon;
    public int price;
    public Sprite[] requiredItemsIcon;
    public string[] requiredItems;  // Danh sách nguyên liệu
    public int[] requiredAmounts;   // Số lượng tương ứng
    public int damage;
    public int durability;
}
