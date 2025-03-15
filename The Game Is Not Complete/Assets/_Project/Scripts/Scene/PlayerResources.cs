using System;
using UnityEngine;

public class PlayerResources : Singleton<PlayerResources> , IDataPersistance
{
    public static event Action OnHealthChanged;
    [SerializeField] private int currentHealth;
    [SerializeField] private int score;
    [SerializeField] private int systemPoints;
    [SerializeField] private int maxHealth = 100;
    public int GetCurrentHealth() {  return currentHealth; }
    public int GetMaxHealth() { return maxHealth; }
    public int GetScore() { return score; }
    public int GetSystemPoints() { return systemPoints; }

    GameOverEvent _gameOverEvent;

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, int scoreDamage)
    {
        //If you have enough health take damage, otherwise set health to 0 and Die
        currentHealth = (currentHealth > damage) ? currentHealth - damage : 0;
        if (currentHealth == 0) Die();

        //for UI when it takes damage
        OnHealthChanged?.Invoke();

        //Makes sure score doesn't go below 0
        score = (score > scoreDamage) ? score - scoreDamage : 0;

        Debug.Log(currentHealth);
    }

    public void Die()
    {
        if (_gameOverEvent == null)
        {
            _gameOverEvent = Resources.FindObjectsOfTypeAll<GameOverEvent>()[0];
            if (_gameOverEvent == null)
            {
                Debug.LogError("GameOverEvent is not found in the scene!");
                return;
            }
        }

        _gameOverEvent.ShowGameOverScreen();
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
        currentHealth = data.lives;
        score = data.score;
        systemPoints = data.systemPoints;
    }

    public void SaveData(ref GameData data)
    {
        data.lives = currentHealth;
        data.score = score;
        data.systemPoints = systemPoints;
    }
}
