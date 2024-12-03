using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject failurePanel;
    
    public void ShowGameOverPanel()
    {
        gameObject.SetActive(true);
        gameOverPanel.SetActive(true);
        failurePanel.SetActive(false);
    }

    public void ShowFailurePanel()
    {
        gameObject.SetActive(true);
        gameOverPanel.SetActive(false);
        failurePanel.SetActive(true);
    }
    
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ExitGame()
    {
        SceneManager.LoadScene(Scenes.MenuScene.ToString());
    }
    
}