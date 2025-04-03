using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class QuestUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private QuestManager questManager;
    [SerializeField] private DataManagers dataManagers;  // Sử dụng DataManagers để lấy item data
    [SerializeField] private TextMeshProUGUI questDescription;  // Hiển thị thông tin quest hiện tại
    [SerializeField] private TextMeshProUGUI questName;  // Hiển thị tên quest hiện tại
    [SerializeField] private GameObject RequireItemContainer;
    [SerializeField] private GameObject RewardItemContainer;
    [SerializeField] private GameObject QuestWindow;
    private Quest selectedQuest;  // Lưu trữ quest đã chọn

 
    void Update()
    {

    }

    private void Start()
    {
        QuestWindow.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        QuestGiver.NotifyEnableQuestWindow += OnEnableQuestWindow;
    }

    private void OnDisable()
    {
        QuestGiver.NotifyEnableQuestWindow -= OnEnableQuestWindow;
    }

    public void ShowQuest(List<Quest> npcQuests)
    {
        Debug.Log("Show Quest");
        string questText = "Available Quests:\n";
        string nameQuest = "";
        bool hasQuestToDisplay = false;
        bool isAnyQuestActive = false;

        ClearUI(); // Xóa UI cũ trước khi hiển thị nhiệm vụ mới

        // Kiểm tra xem có nhiệm vụ nào đang active không
        foreach (var quest in npcQuests)
        {
            if (quest.isActive) // Nếu có ít nhất 1 quest đang active
            {
                isAnyQuestActive = true;
                break;
            }
        }

        // Nếu có nhiệm vụ active thì hiển thị nhiệm vụ active
        if (isAnyQuestActive)
        {
            foreach (var quest in npcQuests)
            {
                if (quest.isActive) // Hiển thị nhiệm vụ đang active
                {
                    questText += quest.description + "\n";
                    nameQuest = quest.questName;
                    hasQuestToDisplay = true;
                    selectedQuest = quest;  // Lưu quest được chọn vào selectedQuest

                    // Hiển thị các item yêu cầu cho nhiệm vụ
                    foreach (var requiredItem in quest.GetRequirements())
                    {
                        if (string.IsNullOrEmpty(requiredItem.requiredItemName))
                        {
                            Debug.LogWarning("Item yêu cầu trống cho quest: " + quest.questName);
                            continue;
                        }

                        ItemData itemData = dataManagers.GetItemDataByName(requiredItem.requiredItemName);
                        if (itemData != null)
                        {
                            GameObject itemIcon = new GameObject("RequireItemIcon");
                            itemIcon.transform.SetParent(RequireItemContainer.transform);

                            Image iconImage = itemIcon.AddComponent<Image>();
                            iconImage.sprite = itemData.icon;

                            GameObject textObj = new GameObject("QuantityText");
                            textObj.transform.SetParent(itemIcon.transform);
                            TextMeshProUGUI quantityText = textObj.AddComponent<TextMeshProUGUI>();
                            quantityText.text = "X" + requiredItem.requiredItemAmount.ToString();
                            quantityText.fontSize = 24;
                            quantityText.alignment = TextAlignmentOptions.Right;
                        }
                    }

                    // Hiển thị các item phần thưởng cho nhiệm vụ
                    foreach (var reward in quest.GetRewards())
                    {
                        if (string.IsNullOrEmpty(reward.itemName))
                        {
                            Debug.LogWarning("Item phần thưởng trống cho quest: " + quest.questName);
                            continue;
                        }

                        ItemData itemData = dataManagers.GetItemDataByName(reward.itemName);
                        if (itemData != null)
                        {
                            GameObject rewardIcon = new GameObject("RewardItemIcon");
                            rewardIcon.transform.SetParent(RewardItemContainer.transform);

                            Image iconImage = rewardIcon.AddComponent<Image>();
                            iconImage.sprite = itemData.icon;

                            GameObject textObj = new GameObject("RewardQuantityText");
                            textObj.transform.SetParent(rewardIcon.transform);
                            TextMeshProUGUI quantityText = textObj.AddComponent<TextMeshProUGUI>();
                            quantityText.text = "X" + reward.amount.ToString();
                            quantityText.fontSize = 24;
                            quantityText.alignment = TextAlignmentOptions.Right;
                        }
                    }
                }
            }
        }

        // Nếu không có nhiệm vụ active, hiển thị các quest của NPC
        if (!isAnyQuestActive && npcQuests.Count > 0)
        {
            foreach (var quest in npcQuests)
            {
                if (quest != null) // Không kiểm tra trạng thái nữa, chỉ cần chắc chắn quest không null
                {
                    questText += quest.description + "\n";
                    nameQuest = quest.questName;
                    hasQuestToDisplay = true;

                    selectedQuest = quest;  // Lưu quest được chọn vào selectedQuest

                    // Hiển thị các item yêu cầu cho nhiệm vụ
                    foreach (var requiredItem in quest.GetRequirements())
                    {
                        if (string.IsNullOrEmpty(requiredItem.requiredItemName))
                        {
                            Debug.LogWarning("Item yêu cầu trống cho quest: " + quest.questName);
                            continue;
                        }

                        ItemData itemData = dataManagers.GetItemDataByName(requiredItem.requiredItemName);
                        if (itemData != null)
                        {
                            GameObject itemIcon = new GameObject("RequireItemIcon");
                            itemIcon.transform.SetParent(RequireItemContainer.transform);

                            Image iconImage = itemIcon.AddComponent<Image>();
                            iconImage.sprite = itemData.icon;

                            GameObject textObj = new GameObject("QuantityText");
                            textObj.transform.SetParent(itemIcon.transform);
                            TextMeshProUGUI quantityText = textObj.AddComponent<TextMeshProUGUI>();
                            quantityText.text = "X" + requiredItem.requiredItemAmount.ToString();
                            quantityText.fontSize = 24;
                            quantityText.alignment = TextAlignmentOptions.Right;
                        }
                    }

                    // Hiển thị các item phần thưởng cho nhiệm vụ
                    foreach (var reward in quest.GetRewards())
                    {
                        if (string.IsNullOrEmpty(reward.itemName))
                        {
                            Debug.LogWarning("Item phần thưởng trống cho quest: " + quest.questName);
                            continue;
                        }

                        ItemData itemData = dataManagers.GetItemDataByName(reward.itemName);
                        if (itemData != null)
                        {
                            GameObject rewardIcon = new GameObject("RewardItemIcon");
                            rewardIcon.transform.SetParent(RewardItemContainer.transform);

                            Image iconImage = rewardIcon.AddComponent<Image>();
                            iconImage.sprite = itemData.icon;

                            GameObject textObj = new GameObject("RewardQuantityText");
                            textObj.transform.SetParent(rewardIcon.transform);
                            TextMeshProUGUI quantityText = textObj.AddComponent<TextMeshProUGUI>();
                            quantityText.text = "X" + reward.amount.ToString();
                            quantityText.fontSize = 24;
                            quantityText.alignment = TextAlignmentOptions.Right;
                        }
                    }
                }
            }
        }

        // Hiển thị quest trên UI
        if (hasQuestToDisplay)
        {
            questDescription.text = questText;
            questName.text = nameQuest;
        }
        else
        {
            questDescription.text = "No quests available.";
            questName.text = "N/A";
        }


    }



    void ClearUI()
    {
        foreach (Transform child in RequireItemContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in RewardItemContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnEnableQuestWindow()
    {
        QuestWindow.gameObject.SetActive(true);
    }

    public void OnDisableQuestWindow()
    {
        QuestWindow.gameObject.SetActive(false);
    }

    public void OnCompleteQuestPressed()
    {
        Quest quest = questManager.activeQuests.Find(q => q.questName == questName.text);

        if (quest != null)
        {
            // Kiểm tra nếu nhiệm vụ không yêu cầu vật phẩm
            if (quest.GetRequirements().Count == 0)
            {
                // Nếu không có vật phẩm yêu cầu, có thể hoàn thành ngay lập tức
                questManager.CompleteQuest(quest.questName, quest.questGiverID);
                Debug.Log($"Nhiệm vụ '{quest.questName}' đã hoàn thành vì không có vật phẩm yêu cầu.");
            }
            else
            {
                Debug.LogWarning("Một số vật phẩm yêu cầu chưa đủ để hoàn thành nhiệm vụ.");
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy nhiệm vụ cần hoàn thành.");
        }
    }




    public void OnAcceptQuestPressed()
    {
        if (selectedQuest != null)
        {
            string npcID = "NPC_ID";  // Đây là ID thực tế của NPC giao nhiệm vụ
            questManager.ActivateQuest(selectedQuest, npcID);
        }
        else
        {
            Debug.LogWarning("Chưa chọn nhiệm vụ.");
        }
    }

    public void OnAddRequireQuestItem()
    {
        Quest quest = questManager.activeQuests.Find(q => q.questName == questName.text);

        if (quest == null)
        {
            Debug.LogWarning("Không tìm thấy nhiệm vụ trong danh sách active.");
            return;
        }

        bool allItemsAdded = true; // Biến kiểm tra nếu tất cả vật phẩm đã đủ

        // Duyệt qua tất cả các yêu cầu vật phẩm trong nhiệm vụ
        foreach (var requiredItem in quest.GetRequirements())
        {
            // Lấy vật phẩm từ Inventory
            InventoryItem playerItem = InventoryManager.Instance.GetInventory().FindItem(requiredItem.requiredItemName);

            if (playerItem != null)
            {
         
                int playerHas = playerItem.amount;
                int requiredAmount = requiredItem.requiredItemAmount;
                int remainingAmount = requiredItem.GetRemainingAmount(); // Lấy số lượng còn thiếu

                if (remainingAmount > 0)
                {
                    int amountToAdd = Mathf.Min(remainingAmount, playerHas); // Thêm tối đa số vật phẩm thiếu
                    requiredItem.AddProgress(amountToAdd); // Cập nhật tiến độ nhiệm vụ

                    // Thêm vật phẩm vào tiến trình nhiệm vụ qua QuestManager
                    questManager.AddItemToQuestProgress(quest.questName, requiredItem.requiredItemName, amountToAdd);

                    // Xóa vật phẩm khỏi inventory
                    InventoryManager.Instance.GetInventory().RemoveItemByName(requiredItem.requiredItemName, amountToAdd);
                    InventoryManager.Instance.GetInventoryDisplay().UpdateDisplay(InventoryManager.Instance.GetInventory());
                    Debug.Log($"Đã thêm {amountToAdd}x {requiredItem.requiredItemName} vào nhiệm vụ.");
                }

                if (!requiredItem.IsCompleted())
                {
                    allItemsAdded = false;
                }
            }
            else
            {
                allItemsAdded = false;
                Debug.LogWarning($"Không có vật phẩm {requiredItem.requiredItemName} trong inventory.");
            }
        }

        if (allItemsAdded)
        {
            Debug.Log("Tất cả vật phẩm yêu cầu đã được thêm vào nhiệm vụ.");
        }
        else
        {
            Debug.LogWarning("Một số vật phẩm vẫn thiếu.");
        }
    }


}
