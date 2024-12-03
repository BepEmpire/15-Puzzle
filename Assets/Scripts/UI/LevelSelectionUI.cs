using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private Difficulty difficulty;
    
    public void SelectDifficulty()
    {
        PlayerPrefs.SetString(Keys.DIFFICULTY, difficulty.ToString());
        PlayerPrefs.Save();
        
        SceneManager.LoadScene(Scenes.GameScene.ToString());
    }
}