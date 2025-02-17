using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UISelectButton : MonoBehaviour
{

    [Header("Building System")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Transform buildingButtonContainer; // Vùng chứa button
    [SerializeField] private BuildingData[] architectureData;
    [SerializeField] private Button buildingButtonPrefab; // Prefab button

    [Header("SeedUI")]
    [SerializeField] private Transform seedButtonContainer; // Vùng chứa button
    [SerializeField] private Button seedButtonPrefab; // Prefab button

    [Header("Seeds Data")]
    [SerializeField] private List<ItemData> seedDataList; // Danh sách hạt giống

    [Header("Inventory")]
    [SerializeField] Inventory playerInventory;



    [Header("Actions")]
    public static Action<BuildingData> buildButtonPressed;
    public static Action<ItemData> seedButtonPressed;


    private void OnEnable()
    {
        BuildingSystem.generateButton += GenerateBuildingButtons;
        PlayerSnowAbility.generateSeedUIButton += GenerateSeedButtons;
    }


    private void OnDisable()
    {
        BuildingSystem.generateButton -= GenerateBuildingButtons;
        PlayerSnowAbility.generateSeedUIButton -= GenerateSeedButtons;


    }
    private void GenerateBuildingButtons()
    {
        // Xóa button cũ
        foreach (Transform child in buildingButtonContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (BuildingData building in architectureData)
        {
            // Tạo button mới
            Button newButton = Instantiate(buildingButtonPrefab, buildingButtonContainer);
            newButton.name = building.buildingName;

            // Kiểm tra Image component
            Image buttonImage = newButton.GetComponent<Image>();
            if (buttonImage == null)
            {
                Debug.LogError($" Không tìm thấy Image component trên button {newButton.name}");
                continue;
            }

            // Kiểm tra building.icon có null không
            if (building.icon == null)
            {
                Debug.LogError($" Building {building.buildingName} chưa có icon!");
                continue;
            }

            // Gán icon và kiểm tra lại sprite
            buttonImage.sprite = building.icon;
            buttonImage.SetNativeSize();

            // Bật/tắt Image để refresh
            buttonImage.enabled = false;
            buttonImage.enabled = true;

            Debug.Log($" Cập nhật icon cho {building.buildingName}: {building.icon.name} ({building.icon})");

            // Thêm sự kiện click
            newButton.onClick.AddListener(() => buildButtonPressed?.Invoke(building));
        }
    }

    private void GenerateSeedButtons()
    {
        // Xóa các button cũ trước khi tạo mới
        foreach (Transform child in seedButtonContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemData seed in seedDataList)
        {
            // Tạo button mới
            Button newButton = Instantiate(seedButtonPrefab, seedButtonContainer);
            newButton.name = seed.itemName;

      

            // Kiểm tra seed.icon có null không
            if (seed.icon == null)
            {
                Debug.LogError($"❌ Hạt giống {seed.itemName} chưa có icon!");
                continue;
            }

         

          

            // Tìm Transform của Icon trong Prefab
            Transform iconTransform = newButton.transform.Find("Icon");
            if (iconTransform != null)
            {
                // Gán hình ảnh vào Image trong đối tượng con Icon
                Image iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null)
                {
                    iconImage.sprite = seed.icon;  // Gán icon vào Image con trong Icon
                    iconImage.SetNativeSize();
                    iconImage.rectTransform.sizeDelta = new Vector2(100, 100);  // Đảm bảo kích thước cố định cho Icon
                }
                else
                {
                    Debug.LogError($"❌ Không tìm thấy Image component trong đối tượng 'Icon' của button {newButton.name}");
                }
            }
            else
            {
                Debug.LogError($"❌ Không tìm thấy đối tượng con 'Icon' trong {newButton.name}");
            }

            // Thêm sự kiện click để chọn hạt giống
            newButton.onClick.AddListener(() => seedButtonPressed?.Invoke(seed));

            Debug.Log($"✅ Tạo nút {seed.itemName} với icon {seed.icon.name}");
        }
    }




}
