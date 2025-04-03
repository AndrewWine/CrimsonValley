using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button LoadGameButton;
    [SerializeField] private Button ExitGameButton;

    private string worldSavePath;
    private string inventorySavePath;

    private void Start()
    {
        worldSavePath = Path.Combine(Application.persistentDataPath, "worlddata.json");
        inventorySavePath = Path.Combine(Application.persistentDataPath, "inventoryData.txt");

        // Kiểm tra nếu có file save thì mới hiện LoadGameButton
        bool hasSaveFile = File.Exists(worldSavePath) || File.Exists(inventorySavePath);
        LoadGameButton.gameObject.SetActive(hasSaveFile);

        // Gán sự kiện cho các nút
        NewGameButton.onClick.AddListener(StartNewGame);
        LoadGameButton.onClick.AddListener(LoadGame);
        ExitGameButton.onClick.AddListener(ExitGame);
    }

    private void StartNewGame()
    {
        // Xóa toàn bộ file save
        if (File.Exists(worldSavePath))
        {
            File.Delete(worldSavePath);
        }
        if (File.Exists(inventorySavePath))
        {
            File.Delete(inventorySavePath);
        }

        Debug.Log("Bắt đầu trò chơi mới, tất cả dữ liệu đã bị xóa!");

        // Chuyển sang scene game
        SceneManager.LoadScene("Main");
    }

    private void LoadGame()
    {
        Debug.Log("Đang tải trò chơi từ file save...");

        // Chuyển sang scene game trước khi load dữ liệu
        SceneManager.LoadScene("Main");

        // Khi scene game đã load xong, WorldManager và InventoryManager sẽ tự load dữ liệu của chúng từ file save.
    }

    private void ExitGame()
    {
        Debug.Log("Thoát game!");
        Application.Quit();
    }
}
