using System;
using UnityEngine;
using UnityEngine.UI;

public class CheckUICageStatus : MonoBehaviour
{
    [Header("Elements UI")]
    [SerializeField] private Button feedButton;
    [SerializeField] private Button harvestButton;

    [Header("Actions")]
    public static Action<int> FeedButton;
    public static Action TakeProduceButton;

    private Cage currentCage; // Lưu Cage đang tương tác

    protected virtual void Start()
    {
        // 🏗 Tìm GameObject có tên "CageUI"
        GameObject cageUI = GameObject.Find("CageUI");
        if (cageUI == null)
        {
            Debug.LogError("⚠ Không tìm thấy GameObject có tên 'CageUI' trong scene!");
            return;
        }

        // 🔍 Tìm các component con trong "CageUI"
       
        feedButton = cageUI.transform.Find("FeedButton")?.GetComponent<Button>();
        harvestButton = cageUI.transform.Find("HarvestButton")?.GetComponent<Button>();

        // Kiểm tra nếu thiếu component nào
   
        if (feedButton == null) Debug.LogWarning(" Không tìm thấy 'FeedButton' trong CageUI!");
        if (harvestButton == null) Debug.LogWarning(" Không tìm thấy 'HarvestButton' trong CageUI!");

        InitializeSettingsUI();
    }

    protected virtual void InitializeSettingsUI()
    {
       
        if (feedButton != null) feedButton.gameObject.SetActive(false);
       
        if (harvestButton != null) harvestButton.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("🔍 Đã vào trigger với: " + other.gameObject.name);

        if (other.CompareTag("Cage"))
        {
            Debug.Log("🐔 Va chạm với chuồng gà!");
            Cage cage = other.GetComponent<Cage>();
            if (cage != null)
            {
                currentCage = cage;
                UpdateCageUI(cage.state);
            }
            else
            {
                Debug.LogWarning("Cage không có script Cage!");
            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cage") && currentCage != null)
        {
            currentCage = null;
            ResetCageUI();
        }
    }

    private void UpdateCageUI(CageState state)
    {
        bool isHungry = state == CageState.Hungry;
        bool isReadyToHarvest = state == CageState.TakeProduce;

       
        if (feedButton != null) feedButton.gameObject.SetActive(isHungry);

       
        if (harvestButton != null) harvestButton.gameObject.SetActive(isReadyToHarvest);
    }

    private void ResetCageUI()
    {
       
        if (feedButton != null) feedButton.gameObject.SetActive(false);
       
        if (harvestButton != null) harvestButton.gameObject.SetActive(false);
    }

    public void FeedAnimal()
    {
        if (currentCage != null)
        {
            FeedButton?.Invoke(10);
            currentCage.ReduceTimeProduce(10);
            currentCage.state = CageState.Empty;  // Đặt lại trạng thái sau khi thu hoạch
            UpdateCageUI(currentCage.state);
        }
    }

    public  void TakeProduce()
    {
        if (currentCage != null && currentCage.state == CageState.TakeProduce)
        {
            currentCage.state = CageState.Empty;  // Đặt lại trạng thái sau khi thu hoạch
            ResetCageUI();
            TakeProduceButton?.Invoke();
        }
    }
}
