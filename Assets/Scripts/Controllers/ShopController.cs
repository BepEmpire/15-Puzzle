using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [Header("Shop Configuration")]
    [SerializeField] private int[] backgroundPrices = { 0, 500, 1000, 1500 };
    [SerializeField] private Button[] backgroundButtons;
    [SerializeField] private TextMeshProUGUI[] buttonTexts;

    [Header("UI Navigation")] 
    [SerializeField] private GameObject shopPage1;
    [SerializeField] private GameObject shopPage2;
    [SerializeField] private Button previousPageButton;
    [SerializeField] private Button nextPageButton;

    private int _currentBackgroundId;

    private void Start()
    {
        InitializeShop();
    }

    public void BuyOrSelectBackground(int backgroundId)
    {
        bool isPurchased = PlayerPrefs.GetInt("BackgroundPurchased_" + backgroundId, 0) == 1;
        
        if (!isPurchased && WalletManager.Instance.SpendCoins(backgroundPrices[backgroundId]))
        {
            PlayerPrefs.SetInt("BackgroundPurchased_" + backgroundId, 1);
        }
        
        if (isPurchased || backgroundId == 0)
        {
            SelectBackground(backgroundId);
        }

        UpdateShopUI();
    }

    private void SelectBackground(int backgroundId)
    {
        _currentBackgroundId = backgroundId;
        PlayerPrefs.SetInt("SelectedBackground", backgroundId);
        PlayerPrefs.Save();
    }
    
    private void InitializeShop()
    {
        PlayerPrefs.SetInt("BackgroundPurchased_0", 1);
        
        _currentBackgroundId = PlayerPrefs.GetInt("SelectedBackground", 0);
        
        nextPageButton.onClick.AddListener(() => SwitchPage(2));
        previousPageButton.onClick.AddListener(() => SwitchPage(1));
        
        UpdateShopUI();
        ShowPage(1);
    }
    
    private void UpdateShopUI()
    {
        for (int i = 0; i < backgroundButtons.Length; i++)
        {
            bool isPurchased = i == 0 || PlayerPrefs.GetInt("BackgroundPurchased_" + i, 0) == 1;
            bool isSelected = i == _currentBackgroundId;
            
            backgroundButtons[i].interactable = isPurchased || WalletManager.Instance.TotalCoins >= backgroundPrices[i];
            
            if (isSelected)
            {
                buttonTexts[i].text = "Selected";
            }
            else if (isPurchased)
            {
                buttonTexts[i].text = "Select";
            }
            else
            {
                buttonTexts[i].text = $"{backgroundPrices[i]}";
            }
        }
    }
    
    private void SwitchPage(int pageNumber)
    {
        ShowPage(pageNumber);
    }

    private void ShowPage(int pageNumber)
    {
        bool isPage1 = pageNumber == 1;

        shopPage1.SetActive(isPage1);
        shopPage2.SetActive(!isPage1);

        previousPageButton.gameObject.SetActive(!isPage1);
        nextPageButton.gameObject.SetActive(isPage1);
    }
}
