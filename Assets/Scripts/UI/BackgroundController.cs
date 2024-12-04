using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private Sprite[] backgroundSprites;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        LoadSelectedBackground();
    }

    private void LoadSelectedBackground()
    {
        int selectedBackgroundId = PlayerPrefs.GetInt(Keys.SELECTED_BACKGROUND, 0);
        
        if (selectedBackgroundId >= 0 && selectedBackgroundId < backgroundSprites.Length)
        {
            _image.sprite = backgroundSprites[selectedBackgroundId];
        }
        else
        {
            Debug.LogError("Invalid background ID: " + selectedBackgroundId);
        }
    }
}