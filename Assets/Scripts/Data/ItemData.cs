using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " Crop Data ", menuName ="Scriptable Objects/Crop", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("Settings")]
    public Item itemPrefab;
    public ItemType itemType;
    public Sprite icon;
    public int price;

}
