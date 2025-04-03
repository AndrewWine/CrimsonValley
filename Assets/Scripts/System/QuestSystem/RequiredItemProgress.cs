[System.Serializable]
public class RequiredItemProgress
{
    public string requiredItemName;
    public int requiredItemAmount;
    public int currentAmount;  // Lưu trữ số lượng vật phẩm đã được thêm vào

    // Hàm kiểm tra xem vật phẩm này đã đủ chưa
    public bool IsCompleted()
    {
        return currentAmount >= requiredItemAmount;
    }

    // Cập nhật tiến độ khi thêm vật phẩm vào
    public void AddProgress(int amountToAdd)
    {
        currentAmount += amountToAdd;
        if (currentAmount > requiredItemAmount) // Giới hạn nếu số lượng vượt quá yêu cầu
            currentAmount = requiredItemAmount;
    }
}
