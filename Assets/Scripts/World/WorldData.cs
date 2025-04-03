
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldData
{
    public string gameDateTime;
    public List<PlacedItemData> placedItems = new List<PlacedItemData>();
    public List<PlacedBuildingData> placedBuildings = new List<PlacedBuildingData>();
    public List<DestroyedTreeData> destroyedTrees = new List<DestroyedTreeData>(); // L?u danh sách cây b? phá h?y

}

[Serializable]
public class ChunkData
{
    public List<PlacedItemData> placedItems = new List<PlacedItemData>();
    public List<PlacedBuildingData> placedBuildings = new List<PlacedBuildingData>();
}


