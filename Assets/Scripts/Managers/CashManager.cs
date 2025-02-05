using System;
using UnityEngine;
using TMPro;

public class CashManager : MonoBehaviour
{
    public static CashManager instance;

    [Header("Settings")]
    [SerializeField] private int coins;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();
        UpdateCoinsContainers();
    }
    public bool UseCoins(int amount)
    {
        if (coins >= amount)
        {
            AddCoins(-amount);
            return true;
        }
        return false; // Insufficient coins
    }
    public void AddCoins(int amount)
    {
        coins += amount; // Update coins
        SaveData(); // Save changes
        UpdateCoinsContainers(); // Update UI
    }

    public int GetCoins()
    {
        return coins;
    }

    private void UpdateCoinsContainers()
    {
        GameObject[] coinContainers = GameObject.FindGameObjectsWithTag("CoinsAmount");

        foreach (GameObject coinContainer in coinContainers)
            coinContainer.GetComponent<TextMeshProUGUI>().text = coins.ToString();
    }

    private void LoadData()
    {
        coins = PlayerPrefs.GetInt("Coins", 0); // Default to 0 if no data exists
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
    }
}
