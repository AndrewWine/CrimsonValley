using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotifyMessage : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject failMessage;
    [SerializeField] private GameObject completeMessage;
    [SerializeField] private GameObject itemProcess;
    private Image itemIcon;
    private TextMeshProUGUI itemCount;

    private void Start()
    {
        ConfigUI();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<ShowItemPickup>(OnShowItemPickup);
        BuildingSystem.NotifyFailMessage += TriggerFailMessage;
        QuestManager.NotifyCompleteMessage += TriggerCompleteMessage;
        QuestManager.NotifyFailMessage += TriggerFailMessage;
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ShowItemPickup>(OnShowItemPickup);
        BuildingSystem.NotifyFailMessage -= TriggerFailMessage;
        QuestManager.NotifyCompleteMessage -= TriggerCompleteMessage;
        QuestManager.NotifyFailMessage -= TriggerFailMessage;
    }

    private void ConfigUI()
    {
        failMessage.SetActive(false);
        completeMessage.SetActive(false);
        itemProcess.SetActive(false);
        itemIcon = itemProcess.GetComponentInChildren<Image>();
        itemCount = itemProcess.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void TriggerFailMessage()
    {
        StartCoroutine(ShowTemporaryMessage(failMessage));
    }

    public void TriggerCompleteMessage()
    {
        StartCoroutine(ShowTemporaryMessage(completeMessage));
    }

    // Khi nhận sự kiện từ EventBus, gọi hàm hiển thị icon
    private void OnShowItemPickup(ShowItemPickup eventData)
    {
        TriggerItemIcon(eventData);
    }

    public void TriggerItemIcon(ShowItemPickup itemData)
    {
        ItemData data = DataManagers.instance.GetItemDataByName(itemData.itemName);

        if (data != null)
        {
            itemIcon.sprite = data.icon;
            itemIcon.rectTransform.sizeDelta = new Vector2(100f, 100f); // ⚡ Đặt size icon

            itemCount.text = "x" + itemData.itemAmount.ToString();
            StartCoroutine(ShowTemporaryMessage(itemProcess));
        }
        else
        {
            Debug.LogError($"Không tìm thấy dữ liệu cho vật phẩm: {itemData.itemName}");
        }
    }


    private IEnumerator ShowTemporaryMessage(GameObject target)
    {
        target.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        target.gameObject.SetActive(false);
    }
}
