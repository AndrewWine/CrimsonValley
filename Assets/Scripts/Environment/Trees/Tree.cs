using System.Collections;
using UnityEngine;
using System;

public class Tree : Item, IDamageAble
{
    [Header("Elements")]
    [SerializeField] private GameObject TopTree;
    [SerializeField] private GameObject TrunkTree;
    [SerializeField] private ItemData itemData;
    [SerializeField] private ParticleSystem woodParticles;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Rigidbody rbTopTree;
    [SerializeField] private Collider trunkCollider;

    [Header("Settings")]
    [SerializeField] private float TopTreeHealth;
    [SerializeField] private float TrunkTreeHealth;
    [SerializeField] private int dropAmount;



    [SerializeField] private bool isTopTree;
    private bool isPlayingParticles = false; // Ngăn việc bật/tắt liên tục

    private void Start()
    {
        // Tìm GameObject có tên "InventoryManager" rồi lấy component InventoryManager
        GameObject inventoryObj = GameObject.Find("InventoryManager");
        if (inventoryObj != null)
        {
            inventoryManager = inventoryObj.GetComponent<InventoryManager>();
        }
        else
        {
            Debug.LogError("InventoryManager không được tìm thấy!");
        }

        InitializedStats();
    }


   
    private void Update()
    {
        if (TopTreeHealth <= 0 && isTopTree)
        {
            ExecuteTopTree();
        }
        if (TrunkTreeHealth <= 0 && !isTopTree)
        {
            ExecuteTrunkTree();
        }
    }

    private void InitializedStats()
    {
        isTopTree = true;
        TopTree.SetActive(true);
        TrunkTree.SetActive(true);
        rbTopTree.isKinematic = true;
        trunkCollider.isTrigger = true; // Ban đầu không thể va chạm
        woodParticles.gameObject.SetActive(false);
    }

    private void ExecuteTopTree()
    {
        PlayParticles(); // Sử dụng hàm kiểm tra trước khi bật

        rbTopTree.isKinematic = false;
        rbTopTree.AddForce(Vector3.right * 0.5f, ForceMode.Impulse); // Đẩy cây ngã
        PickedWood();
        isTopTree = false;
        StartCoroutine(EnableTrunkPhysics());

        Destroy(TopTree, 3.5f); // Xóa phần ngọn sau 2 giây
    }

    private IEnumerator EnableTrunkPhysics()
    {
        yield return new WaitForSeconds(1f);
        trunkCollider.isTrigger = false;
    }

    private void ExecuteTrunkTree()
    {
        PlayParticles(); // Sử dụng hàm kiểm tra trước khi bật

        TrunkTree.SetActive(false);
        PickedWood();

        Destroy(gameObject);
    }

    private void PickedWood()
    {
        woodParticles.transform.parent = null;
      
        inventoryManager.PickUpItemCallBack(itemData.itemName, dropAmount);

    }

    private void PlayParticles()
    {
        if (!isPlayingParticles) // Kiểm tra nếu hạt chưa được kích hoạt
        {
            isPlayingParticles = true;
            woodParticles.gameObject.SetActive(true);
            woodParticles.Play();
            Invoke("DisableParticles", 1f); // Để 1.5s trước khi tắt
        }
    }

    private void DisableParticles()
    {
        woodParticles.gameObject.SetActive(false);
        isPlayingParticles = false; // Reset lại trạng thái
    }

    public void TakeDamage(float target)
    {
        if (isTopTree)
        {
            ShakeTree();
            TopTreeHealth -= target;
        }
        else
        {
            TrunkTreeHealth -= target;
        }
    }

    private void ShakeTree()
    {
        LeanTween.rotate(gameObject, new Vector3(0.2f, 0f, 0.2f), 0.09f).setLoopPingPong(1);
    }
}
