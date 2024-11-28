using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionUI : MonoBehaviour
{
    public void SelectDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("SelectedDifficulty", difficulty);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("GameScene");
    }
}
