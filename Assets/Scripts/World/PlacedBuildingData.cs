using System;
using UnityEngine;

[Serializable]
public class PlacedBuildingData
{
    public string buildingName;
    public Vector3 position;
    public Quaternion rotation;

    public PlacedBuildingData(string name, Vector3 pos, Quaternion rot)
    {
        buildingName = name;
        position = pos;
        rotation = rot;
    }
}
