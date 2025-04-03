using UnityEngine;

[System.Serializable]
public class QuestRequirement
{
    public string requiredItemName;
    public int requiredItemAmount;
    public int currentAmount;

    public QuestRequirement(string itemName, int amount)
    {
        requiredItemName = itemName;
        requiredItemAmount = amount;
        currentAmount = 0;
    }

    // Kiểm tra xem vật phẩm này đã đủ chưa
    public bool IsCompleted()
    {
        return currentAmount >= requiredItemAmount;
    }

    // Cập nhật tiến độ khi thêm vật phẩm vào
    public void AddProgress(int amount)
    {
        currentAmount = Mathf.Min(currentAmount + amount, requiredItemAmount);
    }

    // Lấy số lượng còn thiếu để hoàn thành yêu cầu
    public int GetRemainingAmount()
    {
        return requiredItemAmount - currentAmount;
    }
}
