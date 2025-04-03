using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private PlayerBlackBoard blackboard;

    public static PlayerStatusManager Instance;
    public ListEquipment listEquipment;

    private string equippedItemName = ""; // Tên item đang được trang bị

    private void Start()
    {
        Instance = this;
    }

    public ItemData GetEquippedItem(EquipType equipType)
    {
        return listEquipment?.GetEquippedItemByEquipType(equipType);
    }



    public bool IsEquipped(string itemName)
    {
        return equippedItemName == itemName;
    }

    public void AddAxeDamage(int amount)
    {
        blackboard.Axedamage += amount;
        Debug.Log("New damage: " + blackboard.Axedamage);
    }

    public void RemoveAxeDamage(int amount)
    {
        blackboard.Axedamage -= amount;
        Debug.Log("New damage: " + blackboard.Axedamage);
    }

    public void AddPickaxeDamage(int amount)
    {
        blackboard.Pickaxedamage += amount;
        Debug.Log("New damage: " + blackboard.Pickaxedamage);
    }

    public void RemovePickaxeDamage(int amount)
    {
        blackboard.Pickaxedamage -= amount;
        Debug.Log("New damage: " + blackboard.Pickaxedamage);
    }

    public void EquipItem(string itemName)
    {
        equippedItemName = itemName;
        Debug.Log($"{itemName} has been equipped.");
    }

    public void UnEquipItem(string itemName)
    {
        if (equippedItemName == itemName)
        {
            equippedItemName = "";
            Debug.Log($"{itemName} has been unequipped.");
        }
    }

    // Giảm Stamina khi thực hiện hành động (ví dụ: chặt cây, đập đá)
    public void UseStamina(float amount)
    {
        blackboard.stamina -= amount;
        blackboard.stamina = Mathf.Clamp(blackboard.stamina, 0, 100); // Đảm bảo không xuống dưới 0
        Debug.Log($"Stamina changed: {blackboard.stamina}");

        // Phát sự kiện StaminaChanged
        EventBus.Publish(new StaminaChangedEvent(blackboard.stamina));
    }

    public void ResetStamina(float amount)
    {
        blackboard.stamina += amount;
        Debug.Log("Da hoi lai stamina" + blackboard.stamina);
    }
}
