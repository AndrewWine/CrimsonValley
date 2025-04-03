using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class OreRock : MonoBehaviour, IDamageAble
{
    [Header("Ore Properties")]
    public float maxHealth = 20;
    private float currentHealth;

    private Vector3 originalPosition;

    [Header("Actions")]
    public static Action decreasedPickAxeDurability;

    [System.Serializable]
    public struct DropItem
    {
        public ItemData itemData;
        public float dropRate;
        public int dropAmount;
    }

    [Header("Drop Table (Editable)")]
    public List<DropItem> dropTable = new List<DropItem>();

    private void OnEnable()
    {
        currentHealth = maxHealth;
        originalPosition = transform.position;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        PlayerStatusManager.Instance.UseStamina(1);
        decreasedPickAxeDurability?.Invoke();// EquipItem
        ShakeEffect();
        // Lấy item Pickaxe nếu đang được trang bị
        ItemData equippedItem = ListEquipment.Instance?.GetEquippedItemByEquipType(EquipType.Pickaxe);

        if (equippedItem != null)
        {
            equippedItem.durability -= 1;
           // Debug.Log($"{equippedItem.itemName} durability giảm xuống: {equippedItem.durability}");

            // Nếu durability <= 0, gỡ item khỏi trang bị
            if (equippedItem.durability <= 0)
            {
                ListEquipment.Instance.RemoveItem(equippedItem);
                Debug.Log($"{equippedItem.itemName} đã bị hỏng và bị gỡ khỏi trang bị!");
            }
        }

        if (currentHealth <= 0)
        {
            MineOre();
        }
    }

    private void MineOre()
    {
        DropItem? droppedItem = GetRandomDrop();
        if (droppedItem.HasValue)
        {
            InventoryManager.Instance.PickUpItemCallBack(droppedItem.Value.itemData.itemName, droppedItem.Value.dropAmount);
        }


        // Trả về Object Pool
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.ReturnObject(gameObject);
        }
        else
        {
            Debug.LogError("ObjectPool.Instance is null! Make sure ObjectPool exists in the scene.");
        }


        // Đợi 10 phút rồi spawn lại từ Object Pool
        ObjectPool.Instance.StartCoroutine(RespawnOre());
    }

    private void ShakeEffect()
    {
        Vector3 originalPos = transform.position;
        float elapsedTime = 0f;
        float duration = 0.1f;

        while (elapsedTime < duration)
        {
            float x = UnityEngine.Random.Range(-0.1f, 0.1f);
            float y = UnityEngine.Random.Range(-0.1f, 0.1f);
            transform.position = originalPos + new Vector3(x, y, 0);
            elapsedTime += Time.deltaTime;

        }

        transform.position = originalPos;
    }

    private IEnumerator RespawnOre()
    {
        yield return new WaitForSeconds(600f); // Chờ 10 phút

        GameObject newOre = ObjectPool.Instance.GetObject(originalPosition);
        OreRock oreRock = newOre.GetComponent<OreRock>();
        oreRock.ResetOre();
    }

    public void ResetOre()
    {
        currentHealth = maxHealth;
        transform.position = originalPosition;
        gameObject.SetActive(true);
    }

    private DropItem? GetRandomDrop()
    {
        float randomValue = UnityEngine.Random.Range(0f, 100f);
        float cumulativeProbability = 0f;

        foreach (var drop in dropTable)
        {
            cumulativeProbability += drop.dropRate;
            if (randomValue <= cumulativeProbability)
            {
                return drop;
            }
        }
        return null;
    }
}
