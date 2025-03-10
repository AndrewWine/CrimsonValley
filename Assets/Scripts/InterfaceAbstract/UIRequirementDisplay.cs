using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class UIRequirementDisplay : MonoBehaviour
{
    [SerializeField] protected Transform requirementContainer;
    [SerializeField] protected GameObject requiredItemPrefab;

    /// <summary>
    /// Hiển thị danh sách vật phẩm yêu cầu.
    /// </summary>
    /// <param name="requiredItems">Danh sách tên vật phẩm</param>
    /// <param name="requiredAmounts">Danh sách số lượng vật phẩm</param>
    /// <param name="getItemSpriteFunc">Hàm lấy icon từ tên vật phẩm</param>
    public void ShowRequiredItems(string[] requiredItems, int[] requiredAmounts, Func<string, Sprite> getItemSpriteFunc)
    {
        // Xóa danh sách cũ
        foreach (Transform child in requirementContainer)
        {
            Destroy(child.gameObject);
        }

        // Kiểm tra nếu không có nguyên liệu yêu cầu
        if (requiredItems == null || requiredItems.Length == 0)
        {
            requirementContainer.gameObject.SetActive(false);
            return;
        }

        requirementContainer.gameObject.SetActive(true);

        // Lặp qua danh sách nguyên liệu
        for (int i = 0; i < requiredItems.Length; i++)
        {
            GameObject itemUI = Instantiate(requiredItemPrefab, requirementContainer);

            // Gán dữ liệu vào UI
            Image iconImage = itemUI.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI nameText = itemUI.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI amountText = itemUI.transform.Find("Amount").GetComponent<TextMeshProUGUI>();

            // Lấy icon từ hàm được truyền vào
            Sprite itemIcon = getItemSpriteFunc(requiredItems[i]);
            if (itemIcon != null)
            {
                iconImage.sprite = itemIcon;
            }

            nameText.text = requiredItems[i];
            amountText.text = $"x{requiredAmounts[i]}";
        }
    }
}
