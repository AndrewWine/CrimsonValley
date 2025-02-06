using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("Settings")]
    public string itemName; // Tên item
    public ItemType itemType;
    public Item itemPrefab;
    public Sprite icon;
    public int price;
}
