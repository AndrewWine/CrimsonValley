using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerWaterAbility : MonoBehaviour
{
    // Start is called before the first frame update
    [Header(" Elements ")]
    private PlayerAnimator playerAnimator;

    [Header(" Settings ")]
    private CropField currentCropField;
    // Danh sách các CropField đã gieo
    void Start()
    {
       
        WaterParticles.onWaterColloded += WaterCollidedCallback;
        
    }



    private void OnDestroy()
    {
        WaterParticles.onWaterColloded -= WaterCollidedCallback;
    }



    private void WaterCollidedCallback(Vector3[] waterPositions)
    {
        if (currentCropField == null)
            return;

    }



    
}
