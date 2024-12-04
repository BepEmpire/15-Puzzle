using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform gameGrid;
    [SerializeField] private Timer timer;
    [SerializeField] private GameOverPopup gameOverPopup;
    [SerializeField] private bool isTest;
    
    [Header("Level Data")]
    [SerializeField] private LevelData easyLevelData;
    [SerializeField] private LevelData mediumLevelData;
    [SerializeField] private LevelData hardLevelData;
    
    private Transform _emptyCell;
    private List<Transform> _cards = new List<Transform>();

    private const string EMPTY_CELL_NAME = "EmptyCell";
    private const string PUZZLE_SET = "PuzzleSet";
    private const string RESULT_WIN = "ResultWin";
    private const string RESULT_LOSE = "ResultLose";
    
    private LevelData _currentLevelData;
    private bool _timerStarted = false;

    private void Start()
    {
        InitializeGrid();
        ShuffleGrid();
        LoadLevelData();
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
            
            AudioController.Instance.PlaySound(PUZZLE_SET);
            
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

    private void LoadLevelData()
    {
        string selectedDifficulty = PlayerPrefs.GetString(Keys.DIFFICULTY, Difficulty.Easy.ToString());

        switch (selectedDifficulty)
        {
            case nameof(Difficulty.Easy):
                _currentLevelData = easyLevelData;
                break;
            case nameof(Difficulty.Medium):
                _currentLevelData = mediumLevelData;
                break;
            case nameof(Difficulty.Hard):
                _currentLevelData = hardLevelData;
                break;
            default:
                _currentLevelData = easyLevelData;
                break;
        }
        
        Debug.Log($"Difficulty set to {_currentLevelData.levelName}, Time Limit: {_currentLevelData.timeLimit} seconds");
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
        if (timer.GetElapsedTime() <= _currentLevelData.timeLimit)
        {
            Debug.Log("You win!");
            
            WalletManager.Instance.AddCoins(_currentLevelData.reward);
            Debug.Log($"Player rewarded {_currentLevelData.reward} coins for completing the level.");
            gameOverPopup.ShowGameOverPanel();
            AudioController.Instance.PlaySound(RESULT_WIN);
        }
        else
        {
            Debug.Log("Time's up! You failed to complete the level.");
            gameOverPopup.ShowFailurePanel();
            AudioController.Instance.PlaySound(RESULT_LOSE);
        }
    }
}