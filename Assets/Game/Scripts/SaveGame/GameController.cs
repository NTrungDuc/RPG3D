using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        gameManager.LoadGame();
    }

    void OnApplicationQuit()
    {
        gameManager.SaveGame();
    }
}
