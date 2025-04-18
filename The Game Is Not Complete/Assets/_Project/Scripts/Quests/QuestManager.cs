using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class QuestManager : Singleton<QuestManager>, IDataPersistance
{

    [SerializeField] private UIDocument _uiDocument;

    private VisualElement _questContainer;
    private Label _questText;

    private Queue<Quest> quests = new();
    private Quest currentQuest;

    private void Start()
    {
        _uiDocument = GetComponentInChildren<UIDocument>();
        _questContainer = _uiDocument.rootVisualElement.Q<VisualElement>("QuestContainer");
        _questText = _questContainer.Q<Label>("QuestText");
        _questContainer.style.display = DisplayStyle.None;

        InitializeQuests();
    }

    private void OnEnable()
    {
        QuestEvents.OnQuestProgress += NotifyQuestProgress;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        QuestEvents.OnQuestProgress -= NotifyQuestProgress;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            _questContainer.style.display = DisplayStyle.None;
            InitializeQuests();
        }
        else
        {
            UpdateUI(currentQuest?.isComplete ?? false);
        }
    }

    private void InitializeQuests()
    {
        quests.Clear();
        quests.Enqueue(new Quest("Build a Tower", QuestType.BuildTower, 10));
        quests.Enqueue(new Quest("Kill an Enemy", QuestType.KillEnemy, 20));
        quests.Enqueue(new Quest("Upgrade a Tower", QuestType.UpgradeTower, 30));
        quests.Enqueue(new Quest("Collect 1 Item", QuestType.CollectItem, 40));
        quests.Enqueue(new Quest("Use 1 Items", QuestType.UseItem, 50));
        currentQuest = quests.Dequeue();
    }

    public void NotifyQuestProgress(QuestType type)
    {
        if (currentQuest != null && currentQuest.questType == type && !currentQuest.isComplete)
        {
            CompleteCurrentQuest();
        }
    }
    private void CompleteCurrentQuest()
    {
        currentQuest.isComplete = true;
        PlayerResources.Instance.GainSystemPoints(currentQuest.questReward);
        UpdateUI(true);

        Debug.Log($"Quest '{currentQuest.questText}' completed! Reward: {currentQuest.questReward} points.");

        StartCoroutine(ProceedQuestline());
    }

    private IEnumerator ProceedQuestline()
    {
        yield return new WaitForSeconds(2);
        NextQuest();
    }

    private void NextQuest()
    {
        if (quests.Count > 0)
        {
            currentQuest = quests.Dequeue();
            Debug.Log($"Next quest: {currentQuest.questText}");
            UpdateUI(false);
        }
        else
        {
            Debug.Log("All quests completed");
            _questContainer.style.display = DisplayStyle.None;
        }
    }

    private void UpdateUI(bool completed)
    {
        _questContainer.style.display = DisplayStyle.Flex;
        _questText.text = currentQuest.questText;
        _questText.style.color = completed ? Color.green : Color.red;
    }


    public void LoadData(GameData data)
    {

        for (int i = 0; i < data.currentQuestIndex; i++)
        {
            if (quests.Count > 0) quests.Dequeue();
        }

        if (quests.Count > 0)
        {
            currentQuest = quests.Dequeue();
            currentQuest.isComplete = data.questWasCompleted;
            UpdateUI(currentQuest.isComplete);
        }
        else
        {
            currentQuest = null;
            _questContainer.style.display = DisplayStyle.None;
            Debug.Log("All quests completed");
        }
    }

    public void SaveData(ref GameData data)
    {
        data.currentQuestIndex = GetQuestIndex(currentQuest);
        data.questWasCompleted = currentQuest?.isComplete ?? false;
    }

    private int GetQuestIndex(Quest quest)
    {
        int total = 5;
        return total - quests.Count - 1;
    }

}
