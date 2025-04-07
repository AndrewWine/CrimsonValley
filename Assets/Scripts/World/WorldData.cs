
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldData
{
    public string gameDateTime;
  
    public float _timeOfDay;
    public int _dayNumber , _yearNumber , _yearLength;
    public List<PlacedItemData> placedItems = new List<PlacedItemData>();
    public List<PlacedBuildingData> placedBuildings = new List<PlacedBuildingData>();
    public List<DestroyedTreeData> destroyedTrees = new List<DestroyedTreeData>(); 

}

[Serializable]
public class ChunkData
{
    public List<PlacedItemData> placedItems = new List<PlacedItemData>();
    public List<PlacedBuildingData> placedBuildings = new List<PlacedBuildingData>();
}


