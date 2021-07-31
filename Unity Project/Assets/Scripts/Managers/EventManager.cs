using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void ToggleCharacters(int toggle);
    public static ToggleCharacters toggleCharacters;

    public delegate void EndGame();
    public static EndGame endGame;

    public delegate void SetDeodorant(float duration);
    public static SetDeodorant setDeodorant;

    public delegate void PlayerDied();
    public static PlayerDied playerDied;

    public delegate void FirstZombie();
    public static FirstZombie firstZombie;
}