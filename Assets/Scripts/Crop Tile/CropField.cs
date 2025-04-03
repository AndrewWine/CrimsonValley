using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CropField : Item
{
    [Header(" Elements ")]
    [SerializeField] private Transform tilesParent;
    private List<CropTile> cropTiles = new List<CropTile>();
    private List<GameObject> itemPlaced = new List<GameObject>(); //  Thêm danh sách công trình đã đặt


    [Header("Settings")]
    [SerializeField] private ItemData cropData;
    public TileFieldState state;
    private int tilesHarvested;


    [Header("Actions")]
    public static Action onFullySown;
    public static Action<CropField> onFullyWatered;
    public static Action<CropField> onFullyHarvested;



    void Start()
    {
        state = TileFieldState.Empty;
        StoreTiles();
        HarvestAbillity.Harvesting += Harvest;
        Crop.realdyToHarvest += NotifyRipened;

        // Lắng nghe sự kiện thu hoạch từ AnimationTriggerCrop
        AnimationTriggerCrop.onHarvestTriggered += OnHarvestTriggered;
    }

    private void OnDestroy()
    {
        HarvestAbillity.Harvesting -= Harvest;
        Crop.realdyToHarvest -= NotifyRipened;
        AnimationTriggerCrop.onHarvestTriggered -= OnHarvestTriggered;
    }

    public List<CropTile> GetTiles()
    {
        return cropTiles;
    }

    private void NotifyRipened()
    {
        // Kiểm tra tất cả CropTile trong field
        foreach (var tile in cropTiles)
        {
            if (!tile.IsReadyToHarvest()) return; // Nếu còn cây chưa chín, không đổi state
        }

        // Nếu tất cả đều chín, đổi state của field đó
        state = TileFieldState.Ripened;
        Debug.Log(gameObject.name + " is now fully ripened!");
    }



    private void StoreTiles()
    {
        for (int i = 0; i < tilesParent.childCount; i++)
            cropTiles.Add(tilesParent.GetChild(i).GetComponent<CropTile>());
    }

    public void Sow(ItemData cropData)
    {
        PlayerStatusManager.Instance.UseStamina(1); // Mỗi lần dùng công cụ trừ 10 Stamina
        InventoryManager.Instance.GetInventory().RemoveItemByName(cropData.itemName, 1);
        this.cropData = cropData;
        bool atLeastOneSown = false;
        foreach (CropTile cropTile in cropTiles)
        {
            if (cropTile.IsEmpty())
            {
                cropTile.Sow(cropData);
                atLeastOneSown = true;
            }
        }
        if (atLeastOneSown)
        {
            Debug.Log("At least one tile was sown!");
            CheckFullySown();
        }
    }

    private void CheckFullySown()
    {
        foreach (var tile in cropTiles)
        {
            if (tile.IsEmpty()) return; // Nếu còn ô trống, không cập nhật
        }
        state = TileFieldState.Sown;
        Debug.Log("All tiles are now sown!");
    }



    public void FieldFullySown()
    {
        state = TileFieldState.Sown;
    }
    public void Harvest(Transform harvestSphere)
    {
        if (state != TileFieldState.Ripened)
        {
            Debug.LogWarning($"[{gameObject.name}] Không thể thu hoạch, cây chưa chín! State hiện tại: {state}");
            return;
        }

        Debug.Log($"[{gameObject.name}] Đã vào function HARVEST, harvestSphere position: {harvestSphere.position}, scale: {harvestSphere.localScale}");

        float sphereRadius = harvestSphere.localScale.x;

        bool hasHarvested = false; // Kiểm tra xem có ô nào được thu hoạch không

        for (int i = 0; i < cropTiles.Count; i++)
        {
            if (cropTiles[i].IsEmpty())
            {
                Debug.Log($"Tile {i} ({cropTiles[i].gameObject.name}) is empty, bỏ qua.");
                continue;
            }

            float distanceCropTileSphere = Vector3.Distance(harvestSphere.position, cropTiles[i].transform.position);
            Debug.Log($"Tile {i}: Distance = {distanceCropTileSphere}, Sphere Radius = {sphereRadius}");

            Debug.Log($"Tile {i} ({cropTiles[i].gameObject.name}) sẵn sàng thu hoạch!");
            HarvestTile(cropTiles[i]);
            hasHarvested = true;


        }

        if (!hasHarvested)
        {
            Debug.LogWarning($"[{gameObject.name}] Không có ô nào được thu hoạch! Kiểm tra lại khoảng cách hoặc trạng thái cây trồng.");
        }
    }




    public bool IsEmpty()
    {
        return state == TileFieldState.Empty;
    }



    private void HarvestTile(CropTile cropTile)
    {
        //if (!cropTile.IsReadyToHarvest() ) return;

        cropTile.Harvest();
        tilesHarvested++;

        CheckFullyHarvested();
    }

    private void CheckFullyHarvested()
    {
        foreach (var tile in cropTiles)
        {
            if (!tile.IsEmpty()) return;
        }
        FieldFullyHarvested();
    }


    public void FieldFullyHarvested()
    {
        state = TileFieldState.Empty;
        onFullyHarvested?.Invoke(this);
    }

    public void FieldFullyWatered()
    {
        state = TileFieldState.Watered;
    }
    private void OnHarvestTriggered()
    {
        Harvest(GameObject.FindObjectOfType<AnimationTriggerCrop>().transform);
    }

    public List<GameObject> GetItemPlaced()
    {
        return itemPlaced;
    }

    public void ClearPlacedItem()
    {
        Debug.Log($"Xóa {itemPlaced.Count} item trong CropField...");
        foreach (var item in itemPlaced)
        {
            Destroy(item.gameObject);
        }
        itemPlaced.Clear();
        Debug.Log($"Sau khi xóa: {itemPlaced.Count} item");
    }


    public void LoadCropField(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return;

        GameObject placedObject = Instantiate(prefab, position, rotation);
        itemPlaced.Add(placedObject); //  Thêm vào danh sách công trình đã đặt

        Debug.Log($" Đã tải công trình '{prefab.name}' tại {position}!");
    }

}
