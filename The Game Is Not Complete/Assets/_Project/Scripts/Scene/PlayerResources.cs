using System;
using UnityEngine;

public class PlayerResources : Singleton<PlayerResources>, IDataPersistance
{
    public static event Action OnHealthChanged;
    public static event Action OnScoreChanged;
    public static event Action OnSysPointsChanged;

    [SerializeField] private int currentHealth;
    [SerializeField] private int score;
    [SerializeField] private int systemPoints;
    [SerializeField] private int maxHealth = 100;

    public int GetCurrentHealth() { return currentHealth; }
    public int GetMaxHealth() { return maxHealth; }
    public int GetScore() { return score; }
    public int GetSystemPoints() { return systemPoints; }

    GameOverEvent _gameOverEvent;

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke();
    }

    public void TakeDamage(int damage, int scoreDamage)
    {
        currentHealth = (currentHealth > damage) ? currentHealth - damage : 0;
        if (currentHealth == 0) Die();

        OnHealthChanged?.Invoke();

        int newScore = (score > scoreDamage) ? score - scoreDamage : 0;
        if (newScore != score)
        {
            score = newScore;
            OnScoreChanged?.Invoke();
        }

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
        OnSysPointsChanged?.Invoke();
    }

    public bool SpendSystemPoints(int sysPoints)
    {
        if (systemPoints < sysPoints) return false;

        systemPoints -= sysPoints;
        OnSysPointsChanged?.Invoke();
        return true;
    }

    public void GainScore(int scoreToAdd)
    {
        score += scoreToAdd;
        OnScoreChanged?.Invoke();
    }

    public void LoadData(GameData data)
    {
        currentHealth = data.lives;
        score = data.score;
        systemPoints = data.systemPoints;
        OnHealthChanged?.Invoke();
        OnScoreChanged?.Invoke();
        OnSysPointsChanged?.Invoke();
    }

    public void SaveData(ref GameData data)
    {
        data.lives = currentHealth;
        data.score = score;
        data.systemPoints = systemPoints;
    }
}