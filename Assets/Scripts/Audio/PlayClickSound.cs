using UnityEngine;
using UnityEngine.UI;

public class PlayClickSound : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlayClick);
    }

    private void PlayClick()
    {
        AudioController.Instance.PlaySound(Keys.CLICK);
    }
}
