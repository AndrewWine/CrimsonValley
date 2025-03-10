using UnityEngine;

[CreateAssetMenu(fileName = "New Ore", menuName = "CrimsonValley/Ore")]
public class OreData : ItemData
{
    public IngotData ingotToGive; // Ingot nhận được sau khi đào
    public int dropAmount = 1; // Số lượng ingot nhận được

}
