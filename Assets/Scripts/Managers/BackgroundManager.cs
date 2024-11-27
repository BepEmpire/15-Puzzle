using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Image gameplayBackground;
    [SerializeField] private Sprite[] backgroundSprites;

    private void Start()
    {
        LoadSelectedBackground();
    }

    private void LoadSelectedBackground()
    {
        int selectedBackgroundId = PlayerPrefs.GetInt("SelectedBackground", 0); // Default to the first background
        
        if (selectedBackgroundId >= 0 && selectedBackgroundId < backgroundSprites.Length)
        {
            gameplayBackground.sprite = backgroundSprites[selectedBackgroundId];
        }
        else
        {
            Debug.LogError("Invalid background ID: " + selectedBackgroundId);
        }
    }
}
