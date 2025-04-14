using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Collections;

public class AchievementManager : Singleton<AchievementManager>, IDataPersistance
{
    [SerializeField] private GameObject achievementPrefab;

    private Dictionary<string, bool> achievements = new Dictionary<string, bool>()
    {
        { "Score 100 Points", false },
        { "Score 200 Points", false },
        { "Gain 100 System Points", false },
        { "Gain 200 System Points", false },
        { "Complete Wave 1", false },
        { "Complete Wave 2", false },
        { "Complete Wave 3", false }
    };

    private void OnEnable()
    {
        // Subscribing to events
        PlayerResources.OnScoreChanged += CheckScoreAchievements;
        PlayerResources.OnSysPointsChanged += CheckSysPointsAchievements;
        SceneController.OnWaveCompleted += CheckWaveAchievements;
    }

    private void OnDisable()
    {
        // Unsubscribing to events
        PlayerResources.OnScoreChanged -= CheckScoreAchievements;
        PlayerResources.OnSysPointsChanged -= CheckSysPointsAchievements;
        SceneController.OnWaveCompleted -= CheckWaveAchievements;
    }

    //Score achievements
    private void CheckScoreAchievements()
    {
        if (PlayerResources.Instance.GetScore() >= 100)
        {
            UnlockAchievement("Score 100 Points");
        }
        if (PlayerResources.Instance.GetScore() >= 200)
        {
            UnlockAchievement("Score 200 Points");
        }
    }

    //System point achievements
    private void CheckSysPointsAchievements()
    {
        if (PlayerResources.Instance.GetSystemPoints() >= 100)
        {
            UnlockAchievement("Gain 100 System Points");
        }
        if (PlayerResources.Instance.GetSystemPoints() >= 200)
        {
            UnlockAchievement("Gain 200 System Points");
        }
    }

    //Waves completed achievements
    private void CheckWaveAchievements(int waveNumber)
    {
        if (waveNumber == 1)
        {
            UnlockAchievement("Complete Wave 1");
        }
        if (waveNumber == 2)
        {
            UnlockAchievement("Complete Wave 2");
        }
        if (waveNumber == 3)
        {
            UnlockAchievement("Complete Wave 3");
        }
    }

    public void UnlockAchievement(string id)
    {
        // Ensure the achievement is in the list
        if (!achievements.ContainsKey(id)) return;
        if (achievements[id]) return;

        //Unlock the achievement and display it
        achievements[id] = true;

        //Remove this when we have UI for achievement
        DisplayAchievement(id);
        Debug.Log("Achievement Unlocked: " + id);
    }

    //Remove this when we have UI for achievement
    public void DisplayAchievement(string id)
    {
        achievementPrefab.SetActive(true);
        achievementPrefab.GetComponentInChildren<TextMeshProUGUI>().text = id;
        StartCoroutine(DisableDisplay());
    }

    //Remove this when we have UI for achievement
    public IEnumerator DisableDisplay()
    {
        yield return new WaitForSeconds(4);
        achievementPrefab.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        // Ensure the achievements list is not null and has the same count as the achievements dictionary
        if (data.achievements != null && data.achievements.Count == achievements.Count)
        {
            // Loop through the list and update the achievements dictionary
            int i = 0;
            foreach (KeyValuePair<string, bool> keyValuePair in achievements)
            {
                achievements[keyValuePair.Key] = data.achievements[i];
                i++;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        // Ensure that achievements list in data is initialized
        if (data.achievements == null)
        {
            data.achievements = new List<bool>();
        }

        // Clear existing list
        data.achievements.Clear();

        // Loop through the achievements and store the value
        foreach (KeyValuePair<string, bool> keyValuePair in achievements)
        {
            data.achievements.Add(keyValuePair.Value);
        }
    }


    //TESTING - DELETE THIS
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            PlayerResources.Instance.GainSystemPoints(50);
        }
    }
}