using System;
using UnityEngine;
using UnityEngine.UI;

public class CheckUICageStatus : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button feedButton;
    [SerializeField] private Button harvestButton;

    public static event Action<int> OnFeed;
    public static event Action OnHarvest;

    public Cage activeCage;

    protected virtual void Start()
    {
        GameObject cageUI = GameObject.Find("CageUI");

        if (cageUI == null)
        {
            Debug.LogWarning("CageUI not found!");
            return;
        }

        feedButton = cageUI.transform.Find("FeedButton")?.GetComponent<Button>();
        harvestButton = cageUI.transform.Find("HarvestButton")?.GetComponent<Button>();

        if (!feedButton) Debug.LogWarning("FeedButton not found in CageUI!");
        if (!harvestButton) Debug.LogWarning("HarvestButton not found in CageUI!");

        InitializeUI();
    }

    private void InitializeUI()
    {
        feedButton?.gameObject.SetActive(false);
        harvestButton?.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Cage")) return;

        Cage cage = other.GetComponent<Cage>();

        if (cage == null)
        {
            Debug.LogWarning("Cage component missing!");
            return;
        }

        activeCage = cage;
        UpdateCageUI(cage);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cage") && activeCage != null)
        {
            activeCage = null;
            ResetUI();
        }
    }

    private void UpdateCageUI(Cage cage)
    {
        bool isHungry = cage.state == CageState.Hungry;
        bool isReadyToHarvest = cage.state == CageState.TakeProduce;


        feedButton?.gameObject.SetActive(isHungry);
        harvestButton?.gameObject.SetActive(isReadyToHarvest);
    }

    private void ResetUI()
    {
        feedButton?.gameObject.SetActive(false);
        harvestButton?.gameObject.SetActive(false);
    }

    public void FeedAnimal()
    {
        if (activeCage == null) return;

        OnFeed?.Invoke(50);
        activeCage.HandleFeeding(50);
        activeCage.state = CageState.Empty;
        UpdateCageUI(activeCage);
    }

    public void TakeProduce()
    {
        if (activeCage == null || activeCage.state != CageState.TakeProduce)
        {
            return;
        }

        OnHarvest?.Invoke();
        activeCage.HarvestItem();

        // Reset trạng thái và bộ đếm
        activeCage.state = CageState.Empty;
        activeCage.ResetHarvestTimer();

        UpdateCageUI(activeCage);
    }

}
