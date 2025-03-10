using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSnowAbility : MonoBehaviour
{
    [Header(" Elements ")]
    private PlayerToolSelector playerToolSelector;
    [SerializeField] private GameObject SeedUI;
    private PlayerBlackBoard blackBoard;

    [Header(" Actions ")]
    public static Action<ItemData> SownNotify;
    public static Action generateSeedUIButton;
    private bool unlockCropField = false; // Biến kiểm tra xem có thể gieo hạt hay không

    void Start()
    {
        playerToolSelector = transform.parent.GetComponentInChildren<PlayerToolSelector>();
        blackBoard = GetComponentInParent<PlayerBlackBoard>();
        playerToolSelector.onToolSelected += ToolSelectedCallBack;
        SeedUI.SetActive(false);
        CheckCropFieldState.UnlockCropField += UnlockCropFieldHandler;
        UISelectButton.seedButtonPressed += SelectSeeds;
    }

    private void OnDisable()
    {
        playerToolSelector.onToolSelected -= ToolSelectedCallBack;
        CheckCropFieldState.UnlockCropField -= UnlockCropFieldHandler;
        UISelectButton.seedButtonPressed -= SelectSeeds;

    }
  

    private void ToolSelectedCallBack(PlayerToolSelector.Tool selectedTool)
    {
        if (!playerToolSelector.CanSow())
        {
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
        blackBoard.seed = seed;
        Debug.Log($" Chọn hạt giống: {seed.itemName}");
    }


}
