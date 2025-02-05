using System;
using UnityEngine;

public class CheckCropFieldState : MonoBehaviour
{
    [Header("Actions")]
    public static Action<bool> EnableSowBTTN;
    public static Action<bool> EnableWaterBTTN;
    public static Action<bool> EnableHarvestBTTN;
    public static Action<bool> UnlockCropField;

    private CropField currentCropField; // Chỉ lưu một CropField duy nhất

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Croptile"))
        {
            CropField cropField = other.GetComponent<CropField>();
            if (cropField != null)
            {
                // Chỉ ghi nhận CropField đầu tiên khi người chơi va chạm
                if (currentCropField == null)
                {
                    currentCropField = cropField;
                    UnlockCropField?.Invoke(true); // Mở khóa khi va chạm với một CropField
                }

                // Cập nhật button dựa trên trạng thái của CropField
                UpdateButtons(cropField.state, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Croptile"))
        {
            CropField cropField = other.GetComponent<CropField>();
            if (cropField == currentCropField)
            {
                // Khi rời khỏi CropField hiện tại, thiết lập lại currentCropField
                currentCropField = null;
                UnlockCropField?.Invoke(false); // Đóng khóa khi không còn va chạm với CropField

                // Tắt tất cả các button khi không còn CropField để thao tác
                UpdateButtons(TileFieldState.Empty, false);
            }
        }
    }

    private void UpdateButtons(TileFieldState state, bool enable)
    {
        // Cập nhật trạng thái của các button dựa trên trạng thái của CropField
        EnableSowBTTN?.Invoke(state == TileFieldState.Empty && enable); // Hiển thị nút "Sow" khi trạng thái là Empty
        EnableWaterBTTN?.Invoke(state == TileFieldState.Sown && enable); // Hiển thị nút "Water" khi trạng thái là Sown
        EnableHarvestBTTN?.Invoke(state == TileFieldState.Ripened && enable); // Hiển thị nút "Harvest" khi trạng thái là Watered
    }

    public CropField GetCurrentCropField()
    {
        return currentCropField; // Trả về CropField duy nhất mà người chơi đang va chạm
    }
}
