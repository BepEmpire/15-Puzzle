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

    private const string DEFAULT_BACKGROUND = "BackgroundPurchased_0";
    private const string PURCHASED_BACKGROUND = "BackgroundPurchased_";
    private const string SELECTED = "Selected";
    private const string SELECT = "Select";
    private const string BUY = "Buy";
    
    private int _currentBackgroundId;

    private void Start()
    {
        InitializeShop();
    }

    public void BuyOrSelectBackground(int backgroundId)
    {
        bool isPurchased = PlayerPrefs.GetInt(PURCHASED_BACKGROUND + backgroundId, 0) == 1;
        
        if (!isPurchased && WalletManager.Instance.SpendCoins(backgroundPrices[backgroundId]))
        {
            isPurchased = true;
            PlayerPrefs.SetInt(PURCHASED_BACKGROUND + backgroundId, 1);
            AudioController.Instance.PlaySound(BUY);
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
        PlayerPrefs.SetInt(Keys.SELECTED_BACKGROUND, backgroundId);
        AudioController.Instance.PlaySound(AudioClips.Click.ToString());
        
        PlayerPrefs.Save();
    }
    
    private void InitializeShop()
    {
        PlayerPrefs.SetInt(DEFAULT_BACKGROUND, 1);
        
        _currentBackgroundId = PlayerPrefs.GetInt(Keys.SELECTED_BACKGROUND, 0);
        
        nextPageButton.onClick.AddListener(() => ShowPage(2));
        previousPageButton.onClick.AddListener(() => ShowPage(1));
        
        UpdateShopUI();
        ShowPage(1);
    }
    
    private void UpdateShopUI()
    {
        for (int i = 0; i < backgroundButtons.Length; i++)
        {
            bool isPurchased = i == 0 || PlayerPrefs.GetInt(PURCHASED_BACKGROUND + i, 0) == 1;
            bool isSelected = i == _currentBackgroundId;
            
            backgroundButtons[i].interactable = isPurchased || WalletManager.Instance.TotalCoins >= backgroundPrices[i];
            
            if (isSelected)
            {
                buttonTexts[i].text = SELECTED;
            }
            else if (isPurchased)
            {
                buttonTexts[i].text = SELECT;
            }
            else
            {
                buttonTexts[i].text = $"{backgroundPrices[i]}";
            }
        }
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