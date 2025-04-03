using UnityEngine;

public struct ItemPlacedEvent
{
    public Item item;
    public ItemPlacedEvent(Item item) => this.item = item;
}

public struct ItemDestroyedEvent
{
    public Item item;
    public ItemDestroyedEvent(Item item) => this.item = item;
}


public class BuildingPlacedEvent
{
    public PlacedBuildingData placedBuilding;

    public BuildingPlacedEvent(PlacedBuildingData placedBuilding)
    {
        this.placedBuilding = placedBuilding;
    }
}
