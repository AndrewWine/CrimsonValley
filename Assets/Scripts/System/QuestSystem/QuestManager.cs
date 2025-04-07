using System.Collections.Generic;
using UnityEngine;
using System;
public class QuestManager : MonoBehaviour
{
    [Header("Elements")]
    public List<Quest> activeQuests = new List<Quest>(); // Danh sách nhiệm vụ đang hoạt động
    public QuestGiver[] allQuestGivers; // Danh sách tất cả QuestGivers
    public static Action NotifyFailMessage;//NotifyMessage
    public static Action NotifyCompleteMessage;

    // Kích hoạt nhiệm vụ mới và thêm vào danh sách activeQuests
    public void ActivateQuest(Quest quest, string giverID)
    {
        // Kiểm tra nếu đã có quest active, không nhận quest mới
        if (activeQuests.Exists(q => q.isActive))
        {
            Debug.Log("Bạn đã có một nhiệm vụ active.");
            return;
        }

        // Kích hoạt nhiệm vụ mới
        activeQuests.Add(quest);
        quest.isActive = true;
        quest.questGiverID = giverID;  // Gán ID của NPC đã giao nhiệm vụ
        Debug.Log($"Nhiệm vụ '{quest.questName}' đã được kích hoạt.");
    }

    // Kiểm tra xem nhiệm vụ có thể hoàn thành hay không
    public bool CanCompleteQuest(Quest quest)
    {
        // Kiểm tra tất cả các vật phẩm yêu cầu
        foreach (var requiredItem in quest.GetRequirements())
        {
            if (!requiredItem.IsCompleted())
            {
                //Debug.LogWarning($"Chưa đủ {requiredItem.requiredItemName} để hoàn thành nhiệm vụ.");
                NotifyFailMessage?.Invoke();
                return false;  // Nếu có vật phẩm nào chưa hoàn thành, không thể hoàn thành quest
            }
        }
        return true;  // Nếu tất cả vật phẩm yêu cầu đã hoàn thành
    }

    // Hoàn thành nhiệm vụ
    public void CompleteQuest(string questName, string giverID)
    {
        Quest quest = activeQuests.Find(q => q.questName == questName);

        if (quest != null)
        {
            if (quest.questGiverID == giverID) // Kiểm tra đúng NPC giao nhiệm vụ
            {
                if (CanCompleteQuest(quest)) // Kiểm tra nếu tất cả vật phẩm yêu cầu đã đủ
                {
                    quest.CompleteQuest(); // Hoàn thành nhiệm vụ
                    activeQuests.Remove(quest); // Xóa quest khỏi danh sách activeQuests
                    //Debug.Log($"Nhiệm vụ '{quest.questName}' đã hoàn thành tại NPC '{giverID}'.");
                    NotifyCompleteMessage?.Invoke();
                    // Sau khi hoàn thành quest, gọi RemoveQuestFromAllGivers để xóa quest khỏi tất cả QuestGiver
                    foreach (var questGiver in allQuestGivers)
                    {
                        questGiver.RemoveQuestFromAllGivers(questName, giverID);
                    }
                }
                else
                {
                    Debug.LogWarning("Một số vật phẩm yêu cầu chưa đủ để hoàn thành nhiệm vụ.");
                }
            }
            else
            {
                Debug.LogWarning("Bạn phải quay lại NPC đã giao nhiệm vụ để hoàn thành nó.");
            }
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy nhiệm vụ '{questName}' trong danh sách hoạt động.");
        }
    }

    // Tạo một phương thức để thêm tiến độ vật phẩm cho nhiệm vụ (cập nhật tiến độ yêu cầu vật phẩm)
    public void AddItemToQuestProgress(string questName, string itemName, int amount)
    {
        // Tìm quest cần thêm vật phẩm
        Quest quest = activeQuests.Find(q => q.questName == questName);

        if (quest != null)
        {
            // Tìm item yêu cầu trong nhiệm vụ
            var requiredItem = quest.GetRequirements().Find(r => r.requiredItemName == itemName);

            if (requiredItem != null)
            {
                requiredItem.AddProgress(amount); // Cập nhật tiến độ của yêu cầu vật phẩm
                Debug.Log($"Cập nhật tiến độ vật phẩm {itemName} cho nhiệm vụ {questName}.");
            }
        }
    }
}
