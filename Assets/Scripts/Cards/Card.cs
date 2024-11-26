using UnityEngine;

public class Card : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnCardClicked()
    {
        if (gameManager != null)
        {
            gameManager.TryMoveCard(transform);
        }
    }
}