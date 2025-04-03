using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  
public class CropTile : MonoBehaviour
{

    private TileFieldState state;
    [Header("Elements")]
    private Transform cropParent;
    [SerializeField] private MeshRenderer tileRenderer;
    private Crop crop;
    private ItemData cropData;
    private int dropAmount ;
    [Header("Actions")]
    public static Action<string, int> onCropHarvested;

    [Header("Buff and Debuff CrimsonMoon event")]
    private int DebuffAmount;// cái này sau này dùng cho sự kiện CrimsonMoon nếu kịp làm cốt truyện
    private int BuffAmount ;// cái này sau này dùng cho sự kiện CrimsonMoon nếu kịp làm cốt truyện

    void Start()
    {
        cropParent = GetComponent<Transform>();
        state = TileFieldState.Empty;
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        dropAmount = UnityEngine.Random.Range(3, 10);
        DebuffAmount = UnityEngine.Random.Range(1, 3);
        BuffAmount = UnityEngine.Random.Range(1, 3);
    }

    private void OnDisable()
    {
    }
    public bool IsEmpty()
    {
       return state == TileFieldState.Empty;

    }
    public void Sow(ItemData cropData)
    {
        state = TileFieldState.Sown;

        if (cropData == null || cropData.itemPrefab == null)
        {
            Debug.LogError("CropData or CropPrefab is null!");
            return; // Trả về nếu cropData hoặc cropPrefab là null
        }

        Debug.Log("Sowing");
        state = TileFieldState.Sown;

        // Tạo đối tượng từ prefab
        Crop crop = Instantiate(cropData.itemPrefab, transform.position, Quaternion.identity, cropParent) as Crop;//ép kiểu

        // Cập nhật localScale sau khi khởi tạo
        crop.transform.localScale = new Vector3(
            transform.localScale.x * 1,  // Nhân theo trục X
            transform.localScale.y * 10,  // Nhân theo trục Y
            transform.localScale.z * 1   // Nhân theo trục Z
        );

        this.cropData = cropData;
    }



    public bool IsSown()
    {
        return state == TileFieldState.Sown;
    }

    public void Water()
    {
        crop = GetComponentInChildren<Crop>();
        state = TileFieldState.Watered;
        crop.ScaleUp();
        tileRenderer.gameObject.LeanColor(Color.white * 0.3f, 1).setEase(LeanTweenType.easeOutBack);
    }

    public void Harvest()
    {
        if (crop == null || !crop.IsFullyGrown())
        {
            Debug.Log("Chua the thu hoach");
            return; // Ngăn thu hoạch khi cây chưa phát triển đủ
        }
        InitializeSettings();
        state = TileFieldState.Empty;
        crop.ScaleDown(); //Chỉ thu hoạch khi cây đã lớn đủ
        tileRenderer.gameObject.LeanColor(Color.white, 1).setEase(LeanTweenType.easeOutBack);
        onCropHarvested?.Invoke(cropData.itemName, dropAmount);//InventoryManager
        Debug.Log("Harvest thanh cong");
    }

    public bool IsReadyToHarvest()
    {
        return crop != null && crop.IsFullyGrown();
    }

}
