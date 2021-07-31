using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        EventManager.endGame += EndGame;
        EventManager.playerDied += PlayerDied;
    }

    private void Start()
    {
        EventManager.toggleCharacters(0); //Stop the characters from moving
    }

    public void StartGame()
    {
        UIManager.Instance.BeginDialogue(); //Show the begin game dialogue
    }

    public void EndGame()
    {
        UIManager.Instance.EndGameDialogue(); //Show the ended game ended
    }

    public void PlayerDied()
    {
        UIManager.Instance.GameOver(); //Show game over screen
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}