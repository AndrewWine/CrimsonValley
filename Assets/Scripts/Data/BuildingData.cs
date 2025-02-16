using UnityEngine;
[CreateAssetMenu(fileName = "BuildingRequirement", menuName = "Building System/Building Requirement")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public GameObject buildingPrefab;
    public Sprite icon;
    public Sprite requiredItemsIcon;
    public string[] requiredItems;  // Danh sách nguyên liệu
    public int[] requiredAmounts;   // Số lượng tương ứng
}
