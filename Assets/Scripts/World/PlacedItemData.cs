using System;
using UnityEngine;

[Serializable]
public class PlacedItemData
{
    public string itemName;
    public Vector3 position;
    public Quaternion rotation;

    public PlacedItemData(string Name, Vector3 pos, Quaternion rot)
    {
        itemName = Name;
        position = pos;
        rotation = rot;
    }


}
