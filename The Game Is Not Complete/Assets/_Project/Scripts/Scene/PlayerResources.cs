using UnityEngine;

public class PlayerResources : Singleton<PlayerResources> , IDataPersistance
{
    [SerializeField] private int lives;
    [SerializeField] private int score;
    [SerializeField] private int systemPoints;
    public int GetLives() {  return lives; }
    public int GetScore() { return score; }
    public int GetSystemPoints() { return systemPoints; }


    public void TakeDamage(int damage, int scoreDamage)
    {
        //If you have enough lives take damage, otherwise set lives to 0 and Die
        lives = (lives > damage) ? lives - damage : 0;
        if (lives == 0) Die();

        //Makes sure score doesn't go below 0
        score = (score > scoreDamage) ? score - scoreDamage : 0;

        Debug.Log(lives);
    }

    public void Die()
    {
        //Display lose screen
        FindFirstObjectByType<GameOverEvent>().ShowGameOverScreen();
    }

    public void GainSystemPoints(int sysPoints)
    {
        systemPoints += sysPoints;
    }

    public bool SpendSystemPoints(int sysPoints)
    {
        //Returns false if you don't have enough points
        if (systemPoints < sysPoints) return false;
        else
        {
            systemPoints -= sysPoints;
            return true;
        }
    }

    public void GainScore(int score)
    {
        this.score += score;
    }

    public void LoadData(GameData data)
    {
        lives = data.lives;
        score = data.score;
        systemPoints = data.systemPoints;
    }

    public void SaveData(ref GameData data)
    {
        data.lives = lives;
        data.score = score;
        data.systemPoints = systemPoints;
    }
}
