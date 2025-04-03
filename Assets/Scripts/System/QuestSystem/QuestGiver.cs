using System.Collections.Generic;
using UnityEngine;
using System;
public class QuestGiver : MonoBehaviour
{
    public List<Quest> questsToGive; // Danh sách quest mà NPC này có thể giao
    public QuestManager questManager;
    public static Action NotifyEnableQuestWindow;
    public string questGiverID; // ID duy nhất của NPC

    private void Awake()
    {
        if (string.IsNullOrEmpty(questGiverID))
        {
            questGiverID = gameObject.name; // Đặt ID mặc định là tên GameObject
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Kiểm tra nếu danh sách nhiệm vụ có ít nhất một nhiệm vụ
            if (questsToGive != null && questsToGive.Count > 0)
            {
                // Truyền tất cả nhiệm vụ mà NPC đang giữ
                List<Quest> npcQuests = questsToGive;

                // Kiểm tra có nhiệm vụ nào hợp lệ hay không (ví dụ: kiểm tra nếu quest là ScriptableObject)
                List<Quest> validQuests = npcQuests.FindAll(quest => quest != null && quest is ScriptableObject);

                if (validQuests.Count > 0)
                {
                    QuestUI ui = FindObjectOfType<QuestUI>();
                    ui.ShowQuest(validQuests);  // Truyền danh sách nhiệm vụ hợp lệ vào UI

                    ShowQuestDialogue();  // Hiển thị hộp thoại nhiệm vụ
                    NotifyEnableQuestWindow?.Invoke();  // Gửi thông báo kích hoạt cửa sổ nhiệm vụ
                }
                else
                {
                    Debug.Log("Không có nhiệm vụ hợp lệ để hiển thị.");
                }
            }
            else
            {
                Debug.Log("NPC không có nhiệm vụ nào để giao.");
            }
        }
    }



    void ShowQuestDialogue()
    {
        Debug.Log("Hien thi dialouge");
        // Lọc ra nhiệm vụ mà NPC này đang giữ và chưa active
        List<Quest> availableQuests = questsToGive.FindAll(quest =>  !quest.isCompleted);

        if (availableQuests.Count == 0)
        {
            Debug.Log($"NPC {questGiverID}: Tôi không có nhiệm vụ nào để giao.");
            return;
        }

        Debug.Log($"NPC {questGiverID}: Tôi có {availableQuests.Count} nhiệm vụ.");

        foreach (var quest in availableQuests)
        {
            Debug.Log($"Quest có sẵn: {quest.questName} - {quest.description}");
        }

        // Mở UI để hiển thị danh sách quest của NPC
    }





    public void RemoveQuestFromAllGivers(string questName, string giverID)
    {
        // Duyệt qua tất cả các QuestGiver trong game và xóa quest khỏi questsToGive
        foreach (var questGiver in FindObjectsOfType<QuestGiver>())
        {
            // Kiểm tra nếu questGiver có quest với tên và ID trùng khớp
            Quest questToRemove = questGiver.questsToGive.Find(q => q.questName == questName && q.questGiverID == giverID);

            // Nếu tìm thấy quest cần xóa
            if (questToRemove != null)
            {
                // Xóa quest khỏi danh sách questsToGive của questGiver
                questGiver.questsToGive.Remove(questToRemove);
                Debug.Log($"Quest '{questName}' đã bị xóa khỏi NPC '{giverID}'.");
            }
        }
    }




}
