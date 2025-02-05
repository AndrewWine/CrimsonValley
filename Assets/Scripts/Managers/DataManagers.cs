using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManagers : MonoBehaviour
{
    public static DataManagers instance;
    [Header("Data")]
    [SerializeField] private ItemData[] cropData;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }





    public Sprite GetItemSpriteFromItemType(ItemType itemType)
    {

        for (int i = 0; i < cropData.Length; i++)
        {
            if (cropData[i].itemType == itemType)
                return cropData[i].icon;
        }

        Debug.LogError("No ItemData found of type: " + itemType);
        return null;
    }



    public int GetCropPriceFromCropType(ItemType cropType)
    {
        for (int i = 0; i < cropData.Length; i++)
            if (cropData[i].itemType == cropType)
                return cropData[i].price;
        Debug.LogError("No Itemdata found of that type");
        return 0;
    }
}
