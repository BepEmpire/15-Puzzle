using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform gameGrid;
    [SerializeField] private Timer timer;
    [SerializeField] private GameOverPopup gameOverPopup;
    [SerializeField] private bool isTest;
    
    private Transform _emptyCell;
    private List<Transform> _cards = new List<Transform>();
    
    private bool _timerStarted = false;

    private void Start()
    {
        InitializeGrid();
        ShuffleGrid();
    }
    
    public void TryMoveCard(Transform card)
    {
        Vector2 cardPos = card.GetComponent<RectTransform>().anchoredPosition;
        Vector2 emptyPos = _emptyCell.GetComponent<RectTransform>().anchoredPosition;
        
        if (Vector2.Distance(cardPos, emptyPos) == 200)
        {
            if (!_timerStarted)
            {
                timer.StartTimer();
                _timerStarted = true;
            }
            
            card.GetComponent<RectTransform>().anchoredPosition = emptyPos;
            _emptyCell.GetComponent<RectTransform>().anchoredPosition = cardPos;
            
            int cardIndex = card.GetSiblingIndex();
            int emptyIndex = _emptyCell.GetSiblingIndex();
            
            card.SetSiblingIndex(emptyIndex);
            _emptyCell.SetSiblingIndex(cardIndex);
            
            CheckWinCondition();
        }
    }

    private void InitializeGrid()
    {
        for (int i = 0; i < gameGrid.childCount; i++)
        {
            Transform child = gameGrid.GetChild(i);
            _cards.Add(child);

            if (child.name == "EmptyCell")
            {
                _emptyCell = child;
            }
        }
    }

    private void ShuffleGrid()
    {
        if (isTest) return;
        
        for (int i = 0; i < _cards.Count; i++)
        {
            int randomIndex = Random.Range(0, _cards.Count);
            
            Transform temp = _cards[i];
            _cards[i].SetSiblingIndex(_cards[randomIndex].GetSiblingIndex());
            _cards[randomIndex].SetSiblingIndex(temp.GetSiblingIndex());
        }
    }

    private void CheckWinCondition()
    {
        for (int i = 0; i < _cards.Count - 1; i++)
        {
            Transform card = gameGrid.GetChild(i);

            string expectedName = "Card " + (i + 1).ToString();

            if (card.name != expectedName)
            {
                return;
            }
        }
        
        Debug.Log("You win!");
        timer.StopTimer();
        
        gameOverPopup.ShowGameOverPanel();
    }
}