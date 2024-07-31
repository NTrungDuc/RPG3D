using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidingPuzzle : MonoBehaviour
{
    public List<Button> tiles;
    public List<Button> targetState;
    [SerializeField] private Button emptyButton;
    private int gridSize = 3;
    private void OnEnable()
    {
        PlayerMovement.Instance.LockCursor(true, CursorLockMode.None);
        ShuffleButtons();
    }
    private void OnDisable()
    {
        PlayerMovement.Instance.LockCursor(false, CursorLockMode.Locked);
    }
    void ShuffleButtons()
    {
        int n = tiles.Count;
        System.Random rnd = new System.Random();
        for (int i = 0; i < n; i++)
        {
            int j = rnd.Next(i, n);
            Button temp = tiles[i];
            tiles[i] = tiles[j];
            tiles[j] = temp;
        }


        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].transform.SetSiblingIndex(i);
        }
    }
    public void SlidingTile(Button btn)
    {
        int btnIndex = tiles.IndexOf(btn);
        int emptyIndex = tiles.IndexOf(emptyButton);

        if (IsAdjacent(btnIndex, emptyIndex))
        {
            SwapButtons(btnIndex, emptyIndex);
            if (CheckIfWon())
            {
                Debug.Log("win");
                InventoryManager.Instance.isWinMiniGame = true;
            }
        }
    }

    bool IsAdjacent(int index1, int index2)
    {
        int row1 = index1 / gridSize;
        int col1 = index1 % gridSize;
        int row2 = index2 / gridSize;
        int col2 = index2 % gridSize;

        return (row1 == row2 && Mathf.Abs(col1 - col2) == 1) ||
               (col1 == col2 && Mathf.Abs(row1 - row2) == 1);
    }

    void SwapButtons(int index1, int index2)
    {
        Button temp = tiles[index1];
        tiles[index1] = tiles[index2];
        tiles[index2] = temp;

        //Update pos in hierarchy
        tiles[index1].transform.SetSiblingIndex(index1);
        tiles[index2].transform.SetSiblingIndex(index2);
    }
    private bool CheckIfWon()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i] != targetState[i])
            {
                return false;
            }
        }
        return true;
    }
}
