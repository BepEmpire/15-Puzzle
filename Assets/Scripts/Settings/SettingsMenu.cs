using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;

    [Header("Icons")] 
    [SerializeField] private Sprite soundOnIcon;
    [SerializeField] private Sprite soundOffIcon;
    [SerializeField] private Sprite musicOnIcon;
    [SerializeField] private Sprite musicOffIcon;

    private bool isSoundOn = true;
    private bool isMusicOn = true;

    private void Start()
    {
        LoadSettings();
        UpdateButtonVisuals();
        
        soundButton.onClick.AddListener(ToggleSound);
        musicButton.onClick.AddListener(ToggleMusic);
    }

    private void LoadSettings()
    {
        isSoundOn = PlayerPrefs.GetInt(Keys.SOUND, 1) == 1;
        isMusicOn = PlayerPrefs.GetInt(Keys.MUSIC, 1) == 1;
        
        AudioController.Instance.SetSound(isSoundOn);
        AudioController.Instance.SetMusic(isMusicOn);
    }

    private void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        AudioController.Instance.SetSound(isSoundOn);
        AudioController.Instance.PlaySound(AudioClips.Click.ToString());
        UpdateButtonVisuals();
    }
    
    private void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        AudioController.Instance.SetMusic(isMusicOn);
        AudioController.Instance.PlaySound(AudioClips.Click.ToString());
        UpdateButtonVisuals();
    }

    private void UpdateButtonVisuals()
    {
        soundButton.image.sprite = isSoundOn ? soundOnIcon : soundOffIcon;
        musicButton.image.sprite = isMusicOn ? musicOnIcon : musicOffIcon;
    }
}