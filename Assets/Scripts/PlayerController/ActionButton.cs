using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    [Header("Elements")]
    private PlayerToolSelector playerToolSelector;
    private PlayerBlackBoard blackBoard;

    [Header("Settings")]
    [SerializeField] private Button actionButton;
    [SerializeField] private Button sowButton;
    [SerializeField] private Button waterButton;
    [SerializeField] private Button harvestButton;
    [SerializeField] private Button sleepButton;



    public CropField cropField;

    [Header("Actions")]
    public static Action Hoeing;//HoeAbility
    public static Action Cutting;//HoeAbility
    public static Action Building;//HoeAbility
    public static Action Shoveling;//HoeAbility


    private void Start()
    {
        // Ẩn tất cả các nút khi khởi động
        sowButton.gameObject.SetActive(false);
        waterButton.gameObject.SetActive(false);
        harvestButton.gameObject.SetActive(false);
        sleepButton.gameObject.SetActive(false);
        blackBoard = GetComponentInParent<PlayerBlackBoard>();
        playerToolSelector = GetComponent<PlayerToolSelector>();
        actionButton.onClick.AddListener(DoAction);

        // Đăng ký các sự kiện từ CheckCropFieldState
        CheckGameObject.EnableSowBTTN += EnableSowButton;
        CheckGameObject.EnableWaterBTTN += EnableWaterButton;
        CheckGameObject.EnableHarvestBTTN += EnableHarvestButton;
        CheckGameObject.EnableSleepBTTN += EnableSleepButton;

    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện khi object bị phá hủy (tránh memory leak)
        CheckGameObject.EnableSowBTTN -= EnableSowButton;
        CheckGameObject.EnableWaterBTTN -= EnableWaterButton;
        CheckGameObject.EnableHarvestBTTN -= EnableHarvestButton;
        CheckGameObject.EnableSleepBTTN -= EnableSleepButton;
    }

    public void DoAction()
    {
        if (playerToolSelector.activeTool == PlayerToolSelector.Tool.Hoe)
        {
            // playerAnimator.PlayHoeAnimation();
            blackBoard.hoeButtonPressed = true;
        }
        else if(playerToolSelector.activeTool == PlayerToolSelector.Tool.Axe)
        {
            blackBoard.cutButtonPressed = true;
        }
        else if (playerToolSelector.activeTool == PlayerToolSelector.Tool.Hammer)
        {
            Building?.Invoke();//Building
        }

        else if(playerToolSelector.activeTool == PlayerToolSelector.Tool.Pickaxe)
        {
            blackBoard.miningButtonPressed = true;
            Debug.Log("domining");
        }

        else if (playerToolSelector.activeTool == PlayerToolSelector.Tool.Shovel)
        {
            Shoveling?.Invoke();//ShovelAbility
            Debug.Log("do Shovel");
        }



    }

    public void OnSleepButtonPressed()
    {
        blackBoard.sleepButtonPressed = true;
    }

    //Các hàm này sẽ tự động được gọi khi có sự kiện từ CheckCropFieldState

    public void EnableSleepButton(bool enable)
    {
        sleepButton.gameObject.SetActive(enable);
    }

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

    public void OnJumpButtonPressed()
    {
        blackBoard.jumpButtonPressed = true;    
    }

    public void OnShovelButtonPressed()
    {
        blackBoard.shovelButtonPressed = true;
    }
}
