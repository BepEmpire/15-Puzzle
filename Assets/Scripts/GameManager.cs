using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform gameGrid;
    private Transform _emptyCell;

    private List<Transform> cards = new List<Transform>();

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
            card.GetComponent<RectTransform>().anchoredPosition = emptyPos;
            _emptyCell.GetComponent<RectTransform>().anchoredPosition = cardPos;

            CheckWinCondition();
        }
    }

    private void InitializeGrid()
    {
        for (int i = 0; i < gameGrid.childCount; i++)
        {
            Transform child = gameGrid.GetChild(i);
            cards.Add(child);

            if (child.name == "EmptyCell")
            {
                _emptyCell = child;
            }
        }
    }

    private void ShuffleGrid()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int randomIndex = Random.Range(0, cards.Count);
            
            Transform temp = cards[i];
            cards[i].SetSiblingIndex(cards[randomIndex].GetSiblingIndex());
            cards[randomIndex].SetSiblingIndex(temp.GetSiblingIndex());
        }
    }

    private void CheckWinCondition()
    {
        for (int i = 0; i < cards.Count - 1; i++)
        {
            Transform card = gameGrid.GetChild(i);

            string expectedName = "Card " + (i + 1).ToString();

            if (card.name != expectedName)
            {
                return;
            }
        }
        
        Debug.Log("You win!");
    }
}
