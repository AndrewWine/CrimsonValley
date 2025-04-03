using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    void OnEnable()
    {
        EventBus.Subscribe<QuestCompletedEvent>(OnQuestCompleted);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<QuestCompletedEvent>(OnQuestCompleted);
    }

    void OnQuestCompleted(QuestCompletedEvent questEvent)
    {
        Debug.Log($"UI: Quest '{questEvent.questName}' has been completed! ");
        // Cập nhật giao diện hoặc hiển thị thông báo
    }
}
