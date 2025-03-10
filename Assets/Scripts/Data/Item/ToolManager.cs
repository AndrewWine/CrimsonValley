using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private ItemData itemData;
    [SerializeField] private SpriteRenderer spriteRenderer; // Đảm bảo có tham chiếu tới SpriteRenderer
    [Header("Settings")]
    public int maxDurability;

    private void Start()
    {
        maxDurability = itemData.durability;
    }

    // Function to check and update durability
    private void ChecKDurabilityTool()
    {
        // Assuming itemData has a durability field and maxDurability
        float durabilityPercentage = itemData.durability / maxDurability;

        // Calculate alpha (opacity) based on durability percentage
        float alpha = Mathf.Lerp(1f, 0f, 1 - durabilityPercentage); // Mờ dần khi Durability giảm

        // Update the SpriteRenderer's alpha
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha; // Set alpha to the calculated value
            spriteRenderer.color = color;
        }

        // If durability reaches 0, destroy the item
        if (itemData.durability <= 0)
        {
            DestroyItem();
        }
    }

    // Function to destroy the item
    private void DestroyItem()
    {
        // Code to destroy the item
        Debug.Log("Item destroyed due to 0 durability.");
        Destroy(gameObject); // Or any custom destruction logic
    }
}
