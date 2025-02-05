using System;
using UnityEngine;

public class PlayerSnowAbility : MonoBehaviour
{
    [Header(" Elements ")]
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;
    [SerializeField] GameObject SeedUI;
    private TileFieldState state;

    [Header("Seeds Data")]
    [SerializeField] private ItemData CornData;
    [SerializeField] private ItemData TomatoData;
    private ItemData selectedCropData;

    [Header(" Actions ")]
    public static Action<ItemData> SownNotify;

    private bool unlockCropField = false; // Biến kiểm tra xem có thể gieo hạt hay không

    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();
        playerToolSelector.onToolSelected += ToolSelectedCallBack;
        SeedUI.SetActive(false);
        CheckCropFieldState.UnlockCropField += UnlockCropFieldHandler;
    }

    private void OnDestroy()
    {
        playerToolSelector.onToolSelected -= ToolSelectedCallBack;
        CheckCropFieldState.UnlockCropField -= UnlockCropFieldHandler;
    }

    private void ToolSelectedCallBack(PlayerToolSelector.Tool selectedTool)
    {
        if (!playerToolSelector.CanSow() || !unlockCropField)
        {
            playerAnimator.StopSowAnimation();
            SeedUI.SetActive(false);
        }
        else
        {
            SeedUI.SetActive(true);
        }
    }

    private void UnlockCropFieldHandler(bool unlock)
    {
        unlockCropField = unlock; // Cập nhật trạng thái unlockCropField từ sự kiện
    }

    public void SelectCornSeeds()
    {
        CornData.itemType = ItemType.Corn;
        selectedCropData = CornData;
        Debug.Log("SELECTED CORN SEEDS");

    }

    public void SelectTomatoSeeds()
    {
        TomatoData.itemType = ItemType.Tomato;
        selectedCropData = TomatoData;
        Debug.Log("SELECTED TOMATO SEEDS");

    }

    public void OnSowButtonPressed()
    {
        if (selectedCropData == null)
        {
            Debug.LogError("No seed selected! Please select a seed before sowing.");
            return;
        }

        CropField currentCropField = FindObjectOfType<CheckCropFieldState>()?.GetCurrentCropField();

        if (currentCropField == null)
        {
            Debug.LogError("No CropField detected! Make sure you are in range of a CropField.");
            return;
        }

        if (currentCropField.state != TileFieldState.Empty)
        {
            Debug.LogError("CropField is not empty! Cannot sow on a non-empty field.");
            return;
        }

        Debug.Log("Sowing CropField: " + currentCropField.name);

        // Gọi trực tiếp hàm Sow() của CropField, không dùng sự kiện SownNotify nữa
        currentCropField.Sow(selectedCropData);

        playerAnimator.PlaySowAnimation();
    }



}
