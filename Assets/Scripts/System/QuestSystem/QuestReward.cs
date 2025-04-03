[System.Serializable]
public class QuestReward
{
    public string itemName; // Name of the item as a reward
    public int amount; // Quantity of the reward

    public QuestReward(string itemName, int amount)
    {
        this.itemName = itemName;
        this.amount = amount;
    }
}
