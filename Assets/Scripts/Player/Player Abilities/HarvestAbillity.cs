using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HarvestAbillity : MonoBehaviour
{
    // Start is called before the first frame update
    [Header(" Elements ")]
    [SerializeField] public Transform harvestSphere;
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;
    [Header(" Settings ")]
    private CropField currentCropField;
    private bool canHarvest;

    [Header("Actions")]
    public static Action<Transform> Harvesting;
    void Start()
    {
        playerToolSelector = GetComponent<PlayerToolSelector>();
        playerAnimator = GetComponent<PlayerAnimator>();
        CropField.onFullyHarvested += CropFieldFullyHarvestedCallBack;
    }



    private void OnDestroy()
    {
        CropField.onFullyWatered -= CropFieldFullyHarvestedCallBack;
    }


    private void CropFieldFullyHarvestedCallBack(CropField cropField)
    {
        if (cropField == currentCropField)
            playerAnimator.StopHarvestAnimation();
        canHarvest = true;
    }


   public void OnHarvestButtonPressed()
    {
        playerAnimator.PlayHarvestAnimation();
        Harvesting?.Invoke(harvestSphere);//cropfield
    }

}
