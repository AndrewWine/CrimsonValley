using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Seed, Material, Tool, Produce
}

public enum EquipType // Make enum public
{
    Axe, Pickaxe
}

public enum TileFieldState 
{ 
    Empty,
    Sown, 
    Watered,
    Ripened
}

public enum CageState
{
    Empty,
    Hungry,
    TakeProduce
}


