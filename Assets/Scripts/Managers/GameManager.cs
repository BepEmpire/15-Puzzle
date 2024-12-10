using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private Transform gameGrid;
    [SerializeField] private Timer timer;
    [SerializeField] private GameOverPopup gameOverPopup;
    
    [Header("Levels Data")]
    [SerializeField] private LevelData[] levelsData;
    
    [Header("Test")]
    [SerializeField] private bool isTest;
    
    private Transform _emptyCell;
    private List<Transform> _cards = new List<Transform>();

    private const string EMPTY_CELL_NAME = "EmptyCell";
    
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
            
            AudioController.Instance.PlaySound(AudioClips.PuzzleSet.ToString());
            
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

        for (int i = 0; i < levelsData.Length; i++)
        {
            if (levelsData[i].difficulty.ToString() == selectedDifficulty)
            {
                _currentLevelData = levelsData[i];
                break;
            }
        }
        
        Debug.Log($"Difficulty set to {_currentLevelData.difficulty}, Time Limit: {_currentLevelData.timeLimit} seconds");
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
            AudioController.Instance.PlaySound(AudioClips.ResultWin.ToString());
        }
        else
        {
            Debug.Log("Time's up! You failed to complete the level.");
            gameOverPopup.ShowFailurePanel();
            AudioController.Instance.PlaySound(AudioClips.ResultLose.ToString());
        }
    }
}