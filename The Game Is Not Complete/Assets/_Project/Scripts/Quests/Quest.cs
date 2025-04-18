using UnityEngine;


public enum QuestType
{
    BuildTower,
    KillEnemy,
    UpgradeTower,
    CollectItem,
    UseItem,
}
public class Quest
{
    public string questText;
    public QuestType questType;
    public int questReward;
    public bool isComplete;

    public Quest(string questText, QuestType questType, int questReward)
    {
        this.questText = questText;
        this.questType = questType;
        this.questReward = questReward;
        isComplete = false;
    }
}
