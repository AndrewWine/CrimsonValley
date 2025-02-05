using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CropField : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform tilesParent;
    private List<CropTile> cropTiles = new List<CropTile>();

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
    }


    private void OnDestroy()
    {
        HarvestAbillity.Harvesting -= Harvest;
        Crop.realdyToHarvest -= NotifyRipened;
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
            return;
        float sphereRadius = harvestSphere.localScale.x;

        for (int i = 0; i < cropTiles.Count; i++)
        {
            if (cropTiles[i].IsEmpty())
                continue;

            float distanceCropTileSphere = Vector3.Distance(harvestSphere.position, cropTiles[i].transform.position);

            if (distanceCropTileSphere < sphereRadius && cropTiles[i].IsReadyToHarvest())
            {
                HarvestTile(cropTiles[i]);
            }
        }
    }



    public bool IsEmpty()
    {
        return state == TileFieldState.Empty;
    }



    private void HarvestTile(CropTile cropTile)
    {
        if (!cropTile.IsReadyToHarvest()) return;

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

}
