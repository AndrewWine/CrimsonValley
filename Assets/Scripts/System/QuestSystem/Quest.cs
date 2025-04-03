using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Scriptable Objects/Quest", order = 1)]
public class Quest : ScriptableObject
{
    [Header("Quest Information")]
    public string questName;
    public string description;
    public bool isActive;
    public bool isCompleted;

    [Header("Quest Requirements")]
    public List<QuestRequirement> requirements = new List<QuestRequirement>();

    [Header("Quest Rewards")]
    public List<QuestReward> rewards = new List<QuestReward>();

    public string questGiverID; // ID của NPC đã giao nhiệm vụ

    public List<QuestRequirement> GetRequirements() => requirements;
    public List<QuestReward> GetRewards() => rewards;

    public bool CheckCompletion()
    {
        if (isCompleted) return true;

        foreach (var req in requirements)
        {
            if (!req.IsCompleted()) return false;
        }

        isCompleted = true;
        CompleteQuest();
        return true;
    }

    public void CompleteQuest()
    {
        if (!isCompleted) return;

        Debug.Log($"Quest '{questName}' đã hoàn thành! Đang cấp phần thưởng...");
        GiveRewards();
        EventBus.Publish(new QuestCompletedEvent(questName));
    }

    private void GiveRewards()
    {
        foreach (var reward in rewards)
        {
            EventBus.Publish(new ItemPickedUp(reward.itemName, reward.amount));
            Debug.Log($"Nhận được {reward.amount}x {reward.itemName}");
        }
    }

    public Quest Clone(string giverID)
    {
        Quest clonedQuest = Instantiate(this);
        clonedQuest.requirements = new List<QuestRequirement>();
        foreach (var req in requirements)
        {
            clonedQuest.requirements.Add(new QuestRequirement(req.requiredItemName, req.requiredItemAmount));
        }
        clonedQuest.rewards = new List<QuestReward>(rewards);
        clonedQuest.isActive = false;
        clonedQuest.isCompleted = false;
        clonedQuest.questGiverID = giverID; // Gán NPC đã giao nhiệm vụ

        return clonedQuest;
    }
}
