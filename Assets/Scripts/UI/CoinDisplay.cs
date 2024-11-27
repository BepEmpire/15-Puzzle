using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    private TextMeshProUGUI _coinText;

    private void Awake()
    {
        _coinText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        SetText(WalletManager.Instance.TotalCoins);
        WalletManager.Instance.OnWalletChanged.AddListener(SetText);
    }

    private void OnDestroy()
    {
        WalletManager.Instance.OnWalletChanged.RemoveListener(SetText);
    }

    private void SetText(int coins)
    {
        _coinText.text = coins.ToString();
    }
}
