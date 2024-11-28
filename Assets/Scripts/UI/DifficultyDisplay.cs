using System.Collections;
using System.Collections.Generic;
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
        string selectedDifficulty = PlayerPrefs.GetString("SelectedDifficulty", "Easy");
        _difficultyText.text = selectedDifficulty;
    }

}
