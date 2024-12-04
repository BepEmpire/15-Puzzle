using UnityEngine;
using TMPro;

public class DifficultyDisplay : MonoBehaviour
{
    private TextMeshProUGUI _difficultyText;

    private void Awake()
    {
        _difficultyText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        string selectedDifficulty = PlayerPrefs.GetString(Keys.DIFFICULTY, Difficulty.Easy.ToString());
        _difficultyText.text = selectedDifficulty;
    }
}
