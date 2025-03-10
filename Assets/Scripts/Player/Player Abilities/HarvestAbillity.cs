using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HarvestAbillity : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;
    [Header(" Settings ")]
    private CropField currentCropField;
    private bool canHarvest;

    [Header("Actions")]
    public static Action<Transform> Harvesting;
    void Start()
    {
        playerToolSelector = transform.parent.GetComponentInChildren<PlayerToolSelector>();
        playerAnimator = GetComponentInParent<PlayerAnimator>();
    }

}
