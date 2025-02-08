using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Put all variables you want saved in here
    public int score;
    public int systemPoints;
    public int waveNumber;
    public int lives;

    //When starting a new game these are the initial values assigned
    public GameData()
    {
        score = 0;
        systemPoints = 500;
        waveNumber = 0;
        lives = 100;
    }
}
