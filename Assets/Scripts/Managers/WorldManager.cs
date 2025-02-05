using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;
public class WorldManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform world;
    Chunk[,] grid;

    [Header("Data")]
    private string dataPath;
    private WorldData worldData;

    [Header("Settings")]
    private bool shouldSave;


    private void Awake()
    {
        

    }
   

    private void TrySaveGame()
    {
        if(shouldSave)
        {
            SaveWorld();
            shouldSave = false;
        }
    }


    private void ChunkPriceChangedCallback()
    {
        shouldSave = true;
    }

    private void ChunkUnlockedCallback()
    {
        Debug.Log("cHUNK UNLOCK");
        SaveWorld();    
    }


    private void LoadWorld()
    {
        string data = "";

        if(!File.Exists(dataPath))
        {
            FileStream fs = new FileStream(dataPath, FileMode.Create);
            worldData = new WorldData();
            for (int i = 0; i < world.childCount; i++)
            {
                int chunkInitialPrice = world.GetChild(i).GetComponent<Chunk>().GetInitialPrice();
                worldData.chunkPrices.Add(chunkInitialPrice);
            }
            string worldDataString = JsonUtility.ToJson(worldData , true);
            byte[] worldDataBytes = Encoding.UTF8.GetBytes(worldDataString);
            fs.Write(worldDataBytes);
        }
        else
        {
            data = File.ReadAllText(dataPath);
            worldData = JsonUtility.FromJson<WorldData>(data);

            if(worldData.chunkPrices.Count < world.childCount)
            {
                UpdateData();
            }    
        }
    }

    private void UpdateData()
    {
        //how many chunks are missing in our data
        int missingData = world.childCount - worldData.chunkPrices.Count;
        for (int i = 0;i < missingData;i++)
        {
            int chunkIndex = world.childCount - missingData + i;
            int chunkPrice = world.GetChild(chunkIndex).GetComponent<Chunk>().GetInitialPrice();
            worldData.chunkPrices.Add(chunkPrice);
        }
    }

    private void SaveWorld()
    {
        if (worldData.chunkPrices.Count != world.childCount)
            worldData = new WorldData();
        for(int i = 0; i < world.childCount;i++)
        {
            int chunkCurrentPrice = world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice();
            if(worldData.chunkPrices.Count > i)
                worldData.chunkPrices[i] = chunkCurrentPrice;
            else
                worldData.chunkPrices.Add(chunkCurrentPrice);
        }

        string data = JsonUtility.ToJson(worldData , true);

        File.WriteAllText(dataPath, data);
        Debug.LogWarning("Data saved!");
    }
}
