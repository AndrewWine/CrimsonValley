using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    [Header("Elements")]
    private PlayerToolSelector playerToolSelector;
    private PlayerAnimator playerAnimator;

    [Header("Settings")]
    [SerializeField] private Button actionButton;
    [SerializeField] private Button sowButton;
    [SerializeField] private Button waterButton;
    [SerializeField] private Button harvestButton;

    public CropField cropField;

    [Header("Actions")]
    public static Action Hoeing;//HoeAbility
    public static Action Cutting;//HoeAbility
    public static Action Building;//HoeAbility


    private void Start()
    {
        // Ẩn tất cả các nút khi khởi động
        sowButton.gameObject.SetActive(false);
        waterButton.gameObject.SetActive(false);
        harvestButton.gameObject.SetActive(false);

        playerToolSelector = GetComponent<PlayerToolSelector>();
        playerAnimator = GetComponent<PlayerAnimator>();
        actionButton.onClick.AddListener(DoAction);

        // Đăng ký các sự kiện từ CheckCropFieldState
        CheckCropFieldState.EnableSowBTTN += EnableSowButton;
        CheckCropFieldState.EnableWaterBTTN += EnableWaterButton;
        CheckCropFieldState.EnableHarvestBTTN += EnableHarvestButton;
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện khi object bị phá hủy (tránh memory leak)
        CheckCropFieldState.EnableSowBTTN -= EnableSowButton;
        CheckCropFieldState.EnableWaterBTTN -= EnableWaterButton;
        CheckCropFieldState.EnableHarvestBTTN -= EnableHarvestButton;
    }

    public void DoAction()
    {
        if (playerToolSelector.activeTool == PlayerToolSelector.Tool.Hoe)
        {
            playerAnimator.PlayHoeAnimation();
            Hoeing?.Invoke(); // Kích hoạt sự kiện Hoeing
        }
        else if(playerToolSelector.activeTool == PlayerToolSelector.Tool.Axe)
        {
            Cutting?.Invoke();//CuttingAbility
        }
        else if (playerToolSelector.activeTool == PlayerToolSelector.Tool.Hammer)
        {
            Building?.Invoke();//Building
        }

    }

    //Các hàm này sẽ tự động được gọi khi có sự kiện từ CheckCropFieldState

    public void EnableSowButton(bool enable)
    {
        sowButton.gameObject.SetActive(enable);
    }

    public void EnableWaterButton(bool enable)
    {
        waterButton.gameObject.SetActive(enable);
    }

    public void EnableHarvestButton(bool enable)
    {
        harvestButton.gameObject.SetActive(enable);
    }
}
