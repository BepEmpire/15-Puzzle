using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WalletManager : MonoBehaviour
{
    public static WalletManager Instance;

    public UnityEvent<int> OnWalletChanged;

    private const string WALLET_KEY = "TotalCoins";
    public int TotalCoins { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        TotalCoins = PlayerPrefs.GetInt(WALLET_KEY, 0);
    }

    public void AddCoins(int coins)
    {
        TotalCoins += coins;
        SaveCoins();
        OnWalletChanged?.Invoke(TotalCoins);
    }

    public bool SpendCoins(int amount)
    {
        if (TotalCoins >= amount)
        {
            TotalCoins -= amount;
            SaveCoins();
            OnWalletChanged?.Invoke(TotalCoins);
            return true;
        }

        Debug.Log("Not enough coins to complete this purchase");
        return false;
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt(WALLET_KEY, TotalCoins);
        PlayerPrefs.Save();
    }

    public void RewardCoinsForLevelCompletion()
    {
        const int levelReward = 500;
        AddCoins(levelReward);
        Debug.Log($"Player rewarded {levelReward} coins for completing the level. Total coins: {TotalCoins}");
    }
}
