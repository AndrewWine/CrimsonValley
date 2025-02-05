using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerToolSelector))]
public class PlayerWaterAbility : MonoBehaviour
{
    // Start is called before the first frame update
    [Header(" Elements ")]
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;
    private PlayerSnowAbility playerSnowAbility;
    [Header(" Settings ")]
    private CropField currentCropField;
    // Danh sách các CropField đã gieo
    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerSnowAbility = GetComponent<PlayerSnowAbility>();
        WaterParticles.onWaterColloded += WaterCollidedCallback;
        CropField.onFullyWatered += CropFieldFullyWateredCallBack;
        playerToolSelector = GetComponent<PlayerToolSelector>();
    }



    private void OnDestroy()
    {
        WaterParticles.onWaterColloded -= WaterCollidedCallback;
        CropField.onFullyWatered -= CropFieldFullyWateredCallBack;
    }



    private void CropFieldFullyWateredCallBack(CropField cropField)
    {
        if (cropField == currentCropField)
            playerAnimator.StopWaterAnimation();
    }
    private void WaterCollidedCallback(Vector3[] waterPositions)
    {
        if (currentCropField == null)
            return;


        //currentCropField.WaterCollidedCallBack(waterPositions);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnWaterButtonPressed()
    {
        // Lấy CropField duy nhất mà người chơi đang va chạm
        CropField currentCropField = FindObjectOfType<CheckCropFieldState>().GetCurrentCropField();

        // Chỉ gieo hạt cho CropField mà người chơi đang va chạm và có trạng thái Empty
        if (currentCropField != null && currentCropField.state == TileFieldState.Sown)
        {
            Debug.Log("Sowing CropField: " + currentCropField.name);

            // Gọi trực tiếp hàm Sow() cho mỗi cropTile trong CropField hiện tại
            foreach (var cropTile in currentCropField.GetTiles())
            {
                cropTile.Water(); 
            }

            currentCropField.FieldFullyWatered(); // Đảm bảo cập nhật state
        }
        else
        {
            Debug.Log("No valid CropField to sow");
        }
        playerAnimator.PlayWaterAnimation();
    }

    
}
