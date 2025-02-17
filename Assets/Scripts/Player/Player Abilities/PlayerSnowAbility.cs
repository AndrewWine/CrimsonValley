using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSnowAbility : MonoBehaviour
{
    [Header(" Elements ")]
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;
    [SerializeField] private GameObject SeedUI;
    private ItemData selectedCropData;

    [Header(" Actions ")]
    public static Action<ItemData> SownNotify;
    public static Action generateSeedUIButton;
    private bool unlockCropField = false; // Biến kiểm tra xem có thể gieo hạt hay không

    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();
        playerToolSelector.onToolSelected += ToolSelectedCallBack;
        SeedUI.SetActive(false);
        CheckCropFieldState.UnlockCropField += UnlockCropFieldHandler;
        UISelectButton.seedButtonPressed += SelectSeeds;
    }

    private void OnDestroy()
    {
        playerToolSelector.onToolSelected -= ToolSelectedCallBack;
        CheckCropFieldState.UnlockCropField -= UnlockCropFieldHandler;
        UISelectButton.seedButtonPressed -= SelectSeeds;

    }

    private void ToolSelectedCallBack(PlayerToolSelector.Tool selectedTool)
    {
        if (!playerToolSelector.CanSow())
        {
            playerAnimator.StopSowAnimation();
            SeedUI.SetActive(false);
        }
        else
        {
            SeedUI.SetActive(true);
            generateSeedUIButton?.Invoke();//UISelectButton
        }
    }

    private void UnlockCropFieldHandler(bool unlock)
    {
        unlockCropField = unlock;
    }



    private void SelectSeeds(ItemData seed)
    {
        selectedCropData = seed;
        Debug.Log($" Chọn hạt giống: {seed.itemName}");
    }

    public void OnSowButtonPressed()
    {
        if (selectedCropData == null)
        {
            Debug.LogError(" Chưa chọn hạt giống!");
            return;
        }

        CropField currentCropField = FindObjectOfType<CheckCropFieldState>()?.GetCurrentCropField();

        if (currentCropField == null)
        {
            Debug.LogError(" Không tìm thấy ô đất để gieo trồng!");
            return;
        }

        if (currentCropField.state != TileFieldState.Empty)
        {
            Debug.LogError(" Ô đất không trống!");
            return;
        }

        Debug.Log($"🌱 Gieo hạt {selectedCropData.itemName} tại {currentCropField.name}");
        currentCropField.Sow(selectedCropData);

        playerAnimator.PlaySowAnimation();
    }
}
