using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISeedSelect : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] GameObject CornSeedButton;
    [SerializeField] GameObject TomatoSeedButton;

    [Header("Inventory")]
    [SerializeField] Inventory playerInventory;

    private void Start()
    {
        // Gọi trực tiếp OnAbleToSowSeed với tên của seed
        //OnAbleToSowSeed("Corn");  // Ví dụ gọi cho Corn
        //
        //OnAbleToSowSeed("Tomato");  // Ví dụ gọi cho Tomato
    }

    private void OnDestroy()
    {
       // DataManagers.onSeedDisplay -= OnAbleToSowSeed;  // Đảm bảo không có sự kiện bị rò rỉ
    }

    // Phương thức này sẽ được gọi khi bạn kiểm tra nếu có hạt trong inventory và hiển thị button
    public void OnAbleToSowSeed(string seedName)
    {
        Debug.Log("EnableUIseed for " + seedName);

        // Kiểm tra xem vật phẩm có tồn tại trong inventory hay không trước khi hiển thị button
        bool hasCorn = CheckItemInInventory("Corn");
        bool hasTomato = CheckItemInInventory("Tomato");

        if (seedName == "Corn" && hasCorn)
        {
            CornSeedButton.SetActive(true);
        }
        else
        {
            CornSeedButton.SetActive(false);
        }

        if (seedName == "Tomato" && hasTomato)
        {
            TomatoSeedButton.SetActive(true);
        }
        else
        {
            TomatoSeedButton.SetActive(false);
        }
    }

    // Hàm kiểm tra sự tồn tại của item trong inventory
    private bool CheckItemInInventory(string itemName)
    {
        // Kiểm tra nếu itemName có tồn tại trong inventory và có số lượng > 0
        InventoryItem[] items = playerInventory.GetInventoryItems();
        foreach (var item in items)
        {
            if (item.itemName.Equals(itemName, System.StringComparison.OrdinalIgnoreCase) && item.amount > 0)
            {
                return true;
            }
        }
        return false;
    }
}
