using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance; // Thêm biến instance
    [SerializeField] private DayNightCycle dayNightCycle;

    private string dataPath;
    public WorldData worldData;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Gán instance cho đối tượng đầu tiên
        }
        else
        {
            Destroy(gameObject); // Nếu đã có instance, hủy object này để tránh trùng lặp
            return;
        }
        dataPath = Path.Combine(Application.persistentDataPath, "worlddata.json");
        worldData = new WorldData();
    }

    private IEnumerator Start()
    {

        // Chờ DataManagers khởi tạo
        while (DataManagers.instance == null)
        {
            Debug.LogWarning(" Đợi DataManagers.instance...");
            yield return null;
        }

        Debug.Log(" DataManagers.instance đã có, tiếp tục LoadWorld()");
        StartCoroutine(LoadWorldCoroutine());
    }

    private void OnEnable()
    {
        EventBus.Subscribe<ItemPlacedEvent>(OnItemPlaced);
        EventBus.Subscribe<ItemDestroyedEvent>(OnItemDestroyed);
        EventBus.Subscribe<BuildingPlacedEvent>(OnBuildingPlaced);
        EventBus.Subscribe<TreeDestroyedEvent>(OnTreeDestroyed);
        EventBus.Subscribe<BuildingDestroyedEvent>(OnBuildingDestroyed); 


    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ItemPlacedEvent>(OnItemPlaced);
        EventBus.Unsubscribe<ItemDestroyedEvent>(OnItemDestroyed);
        EventBus.Unsubscribe<BuildingPlacedEvent>(OnBuildingPlaced);
        EventBus.Unsubscribe<TreeDestroyedEvent>(OnTreeDestroyed);
        EventBus.Unsubscribe<BuildingDestroyedEvent>(OnBuildingDestroyed); 

    }



    public void SaveWorld()
    {
        if (worldData == null)
            worldData = new WorldData();
        worldData._timeOfDay = dayNightCycle.timeOfDay;
        worldData._dayNumber = dayNightCycle.dayNumber;
        worldData._yearNumber = dayNightCycle.yearNumber;
        worldData._yearLength = dayNightCycle.yearLength;
        worldData.gameDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        // Tạo danh sách mới và xóa danh sách cũ trong worldData
        List<PlacedItemData> newPlacedItems = new List<PlacedItemData>();
        List<PlacedBuildingData> newPlacedBuildings = new List<PlacedBuildingData>();

        worldData.placedItems.Clear();
        worldData.placedBuildings.Clear();

        // Lưu tất cả các Item (bao gồm cả Tree) trong scene
        foreach (var item in FindObjectsOfType<Item>())
        {
            if (item == null || item.itemData == null || string.IsNullOrEmpty(item.itemData.itemName))
            {
                Debug.LogWarning(" Bỏ qua item không có ItemData hoặc itemName trống!");
                continue;
            }

            newPlacedItems.Add(new PlacedItemData(
                item.itemData.itemName,  // Lấy itemName từ itemData
                item.transform.position,
                item.transform.rotation
            ));
        }

        // Nếu bạn cần lưu thêm thông tin riêng của Tree, bạn có thể xử lý ở đây.
        // Nhưng nếu Tree chỉ là 1 Item, vòng lặp trên đã lưu dữ liệu của chúng.

        // Lưu Building từ BuildingSystem
        BuildingSystem buildingSystem = FindObjectOfType<BuildingSystem>();
        if (buildingSystem != null)
        {
            var placedBuildings = buildingSystem.GetPlacedBuildings();
            if (placedBuildings.Count == 0)
            {
                Debug.LogWarning(" Không có công trình nào để lưu!");
            }
            else
            {
                foreach (var building in placedBuildings)
                {
                    if (building == null) continue;
                    PlacedBuilding placedBuilding = building.GetComponent<PlacedBuilding>();
                    if (placedBuilding == null || placedBuilding.buildingData == null) continue;
                    // Chỉ lưu nếu building không phải là "Player"
                    if (placedBuilding.buildingData.buildingName == "Player") continue;

                    newPlacedBuildings.Add(new PlacedBuildingData(
                        placedBuilding.buildingData.buildingName,
                        building.transform.position,
                        building.transform.rotation
                    ));
                }
            }
        }
        else
        {
            Debug.Log(" Không tìm thấy BuildingSystem, có thể gây mất dữ liệu!");
        }

        // Cập nhật worldData
        worldData.placedItems = newPlacedItems;
        worldData.placedBuildings = newPlacedBuildings;

        string json = JsonUtility.ToJson(worldData, true);
        File.WriteAllText(dataPath, json);

        Debug.Log(" World Saved!");
    }



    public IEnumerator LoadWorldCoroutine()
    {
        Debug.Log(" Bắt đầu LoadWorld...");

        if (!File.Exists(dataPath))
        {
            Debug.LogWarning(" Không tìm thấy file worlddata.json, tạo mới!");
            worldData = new WorldData();
            SaveWorld();
            yield break;
        }

        string data = File.ReadAllText(dataPath);
        if (string.IsNullOrEmpty(data))
        {
            Debug.LogError(" File worlddata.json rỗng! Reset dữ liệu.");
            worldData = new WorldData();
            SaveWorld();
            yield break;
        }

        worldData = JsonUtility.FromJson<WorldData>(data);
        if (worldData == null)
        {
            Debug.LogError(" Lỗi khi đọc JSON, tạo lại worldData.");
            worldData = new WorldData();
            SaveWorld();
            yield break;
        }

        // Xóa tất cả Item hiện có
        Item[] items = FindObjectsOfType<Item>();
        Debug.Log($" Số lượng Item trước khi xóa: {items.Length}");
        for (int i = items.Length - 1; i >= 0; i--)
        {
            Destroy(items[i].gameObject);
        }

        // Xóa tất cả Building hiện có
        BuildingSystem buildingSystem = FindObjectOfType<BuildingSystem>();
        if (buildingSystem != null)
        {
            buildingSystem.ClearPlacedBuildings();
        }
        //else
      //  {
        //  Debug.Log(" Không tìm thấy BuildingSystem!");
       // }

        // Đợi đến cuối frame để chắc chắn các đối tượng cũ đã bị xóa
        yield return new WaitForEndOfFrame();

        // Kiểm tra lại số lượng Item sau khi xóa
        items = FindObjectsOfType<Item>();
        Debug.Log($" Số lượng Item sau khi xóa: {items.Length}");

        // Load lại Item từ dữ liệu save
        foreach (var itemData in worldData.placedItems)
        {
            if (string.IsNullOrEmpty(itemData.itemName))
            {
                Debug.LogWarning(" Bỏ qua item không có tên!");
                continue;
            }

            ItemData itemPrefab = DataManagers.instance.GetItemDataByName(itemData.itemName);
            if (itemPrefab != null && itemPrefab.itemPrefab != null)
            {
                GameObject newItemObj = Instantiate(itemPrefab.itemPrefab.gameObject, itemData.position, itemData.rotation);
                Debug.Log($" Đã tạo {itemData.itemName} tại {itemData.position}");
            }
            else
            {
                Debug.LogError($" Không tìm thấy prefab cho {itemData.itemName}");
            }
        }

        // Load lại Building từ BuildingSystem
        if (buildingSystem != null)
        {
            foreach (var buildingData in worldData.placedBuildings)
            {
                if (buildingData.buildingName == "Player") continue;

                BuildingData prefab = DataManagers.instance.GetBuildingDataByName(buildingData.buildingName);
                if (prefab != null && prefab.buildingPrefab != null)
                {
                    buildingSystem.LoadBuilding(prefab.buildingPrefab, buildingData.position, buildingData.rotation);
                }
                else
                {
                    Debug.LogError($" Không tìm thấy prefab cho {buildingData.buildingName}");
                }
            }
        }

        // Xóa các cây đã bị phá hủy
        foreach (var tree in FindObjectsOfType<Tree>())
        {
            foreach (var destroyedTree in worldData.destroyedTrees)
            {
                if (Vector3.Distance(tree.transform.position, destroyedTree.position) < 0.1f)
                {
                    Destroy(tree.gameObject);
                }
            }
        }

        Debug.Log(" LoadWorld hoàn tất!");
    }



    private void OnBuildingDestroyed(BuildingDestroyedEvent evt)
    {
        if (worldData == null)
            worldData = new WorldData();

        // Kiểm tra xem `evt.building` có `buildingData` hay không
        if (evt.building == null || evt.building.buildingData == null)
        {
            Debug.LogWarning(" Không tìm thấy buildingData, bỏ qua xóa.");
            return;
        }

        string buildingName = evt.building.buildingData.buildingName;
        Vector3 buildingPosition = evt.building.transform.position;

        // Xóa công trình khỏi danh sách lưu trữ
        bool removed = worldData.placedBuildings.RemoveAll(building =>
            building.buildingName == buildingName &&
            Vector3.Distance(building.position, buildingPosition) < 0.01f) > 0;

        if (removed)
        {
            Debug.Log($" Đã xóa {buildingName} khỏi dữ liệu save!");
            SaveWorld(); // Cập nhật file save ngay lập tức
        }
    }

    private void OnItemPlaced(ItemPlacedEvent evt)
    {
        worldData.placedItems.Add(new PlacedItemData(
            evt.item.itemData.itemName,
            evt.item.transform.position,
            evt.item.transform.rotation
        ));
    }
    private void OnTreeDestroyed(TreeDestroyedEvent evt)
    {
        if (worldData == null)
            worldData = new WorldData();

        // Lưu vị trí cây vào danh sách đã bị phá hủy
        worldData.destroyedTrees.Add(new DestroyedTreeData(evt.position));
        SaveWorld();
    }


    private void OnItemDestroyed(ItemDestroyedEvent evt)
    {
        worldData.placedItems.RemoveAll(i =>
            i.itemName == evt.item.itemData.itemName && Vector3.Distance(i.position, evt.item.transform.position) < 0.01f);
        SaveWorld();
    }
    private void OnBuildingPlaced(BuildingPlacedEvent eventData)
    {
        if (eventData.placedBuilding.buildingName == "Player") return;

        worldData.placedBuildings.Add(eventData.placedBuilding);
        Debug.Log($" Công trình {eventData.placedBuilding.buildingName} đã được đặt tại {eventData.placedBuilding.position}");

        SaveWorld(); // GỌI NGAY KHI ĐẶT BUILDING!
    }

    

    private void OnApplicationQuit()
    {
        SaveWorld();
        Debug.Log("ĐÃ LƯU");
    }
}