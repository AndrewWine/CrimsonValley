using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform cropContainersParent;
    [SerializeField] private UIcropContainer uicropContainer;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Configure(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();
        for (int i = 0; i < items.Length; i++)
        {
            UIcropContainer cropContainerInstance = Instantiate(uicropContainer,cropContainersParent);
            Sprite cropIcon = DataManagers.instance.GetItemSpriteFromItemType(items[i].itemType);
            cropContainerInstance.Configure(cropIcon,items[i].amount);
        }
    }

    public void UpdateDisplay(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();
        for (int i = 0;i < items.Length;i++)
        {
            UIcropContainer containerInstance;
            if (i < cropContainersParent.childCount)
            {
                containerInstance = cropContainersParent.GetChild(i).GetComponent<UIcropContainer>();
                containerInstance.gameObject.SetActive(true);         
            }

            else
                containerInstance = Instantiate(uicropContainer, cropContainersParent);

            Sprite cropIcon = DataManagers.instance.GetItemSpriteFromItemType(items[i].itemType);
            containerInstance.Configure(cropIcon, items[i].amount);

        }
        int remainingContainers = cropContainersParent.childCount - items.Length;

        if (remainingContainers <= 0)
            return;

        for(int i = 0; i < remainingContainers; i++)
        {
            cropContainersParent.GetChild(items.Length + i ).gameObject.SetActive(false);
        }
        /*
   while(cropContainersParent.childCount > 0)
   {
       //clear the crop contaienrs parent if there are any ui corp containers
       Transform container = cropContainersParent.GetChild(0);
       container.SetParent(null);
       Destroy(container.gameObject);
   }

   //create the ui crop containers from scratch again

   Configure(inventory);


   for(int i = 0;i < items.Length;i++)
   {
       UIcropContainer cropContainerInstance = Instantiate(uicropContainer, cropContainersParent);
       Sprite cropIcon = DataManagers.instance.GetCropSpriteFromCropType(items[i].croptype)
;
       cropContainerInstance.Configure(cropIcon, items[i].amount);
   }
   */

    }
}
