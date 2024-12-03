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

    private const string EMPTY_CELL_NAME = "EmptyCell";
    
    private const int EASY_REWARD = 100;
    private const int MEDIUM_REWARD = 200;
    private const int HARD_REWARD = 500;
    
    private bool _timerStarted = false;

    private int _currentReward;
    private float _timeLimit;

    private void Start()
    {
        InitializeGrid();
        ShuffleGrid();
        LoadDifficultySettings();
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
            
            AudioController.Instance.PlaySound("PuzzleSet");
            
            CheckWinCondition();
        }
    }

    private void InitializeGrid()
    {
        for (int i = 0; i < gameGrid.childCount; i++)
        {
            Transform child = gameGrid.GetChild(i);
            _cards.Add(child);

            if (child.name == EMPTY_CELL_NAME)
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

    private void LoadDifficultySettings()
    {
        string selectedDifficulty = PlayerPrefs.GetString(Keys.DIFFICULTY, Difficulty.Easy.ToString());

        switch (selectedDifficulty)
        {
            case nameof(Difficulty.Easy):
                _currentReward = EASY_REWARD;
                _timeLimit = 30f;
                break;
            case nameof(Difficulty.Medium):
                _currentReward = MEDIUM_REWARD;
                _timeLimit = 20f;
                break;
            case nameof(Difficulty.Hard):
                _currentReward = HARD_REWARD;
                _timeLimit = 10f;
                break;
            default:
                _currentReward = EASY_REWARD;
                _timeLimit = 30f;
                break;
        }
        
        Debug.Log($"Difficulty set to {selectedDifficulty}, Time Limit: {_timeLimit} seconds");
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

        timer.StopTimer();
        CheckResult();
    }

    private void CheckResult()
    {
        if (timer.GetElapsedTime() <= _timeLimit)
        {
            Debug.Log("You win!");
            
            WalletManager.Instance.AddCoins(_currentReward);
            Debug.Log($"Player rewarded {_currentReward} coins for completing the level.");
            gameOverPopup.ShowGameOverPanel();
            AudioController.Instance.PlaySound("ResultWin");
        }
        else
        {
            Debug.Log("Time's up! You failed to complete the level.");
            gameOverPopup.ShowFailurePanel();
            AudioController.Instance.PlaySound("ResultLose");
        }
    }
}