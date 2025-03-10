using System.Collections;
using UnityEngine;
using System;
using UnityEditor.ShaderGraph;

public class Tree : Item, IHitAble, IDamageAble
{
    [Header("Elements")]
    [SerializeField] private GameObject TopTree;
    [SerializeField] private GameObject TrunkTree;
    [SerializeField] private ItemData itemData;
    [SerializeField] private ParticleSystem woodParticles;
    private Renderer topTreeRenderer;
    private Rigidbody rbTopTree;


    [Header("Settings")]
    [SerializeField] private float TopTreeHealth;
    [SerializeField] private float TrunkTreeHealth;
    [SerializeField] private int dropAmount;


    [Header("Actions")]
    public static Action<string, int> onPickupWood;

    private Color startColor;
    private float fadeDuration = 6f; // Thời gian mờ dần (6 giây)
    private float timeElapsed = 0f;
    private bool isTopFading = false;
    private bool isTrunkFading = false;

    [SerializeField] private bool isTopTree;

    private void Start()
    {
        rbTopTree = TopTree.GetComponentInChildren<Rigidbody>();
        topTreeRenderer = TopTree.GetComponentInChildren<Renderer>();
        startColor = topTreeRenderer.material.color; // Lưu lại màu ban đầu
        InitializedStats();
        caculatedDropAmount();

    }

    private void caculatedDropAmount()
    {
        dropAmount = UnityEngine.Random.Range(2, 5);
    }

    private void Update()
    {
        if (TopTreeHealth <= 0 && !isTopFading)
        {
            Execute();
        }
        if (TrunkTreeHealth <= 0 && !isTrunkFading)
        {
            Execute();
        }

    }

    private void InitializedStats()
    {
        TopTreeHealth = 10;
        TrunkTreeHealth = 5;
        isTopTree = true;
        TopTree.SetActive(true);
        TrunkTree.SetActive(true);
        rbTopTree.isKinematic = true;
    }

    public void Execute()
    {
        if (isTopTree)
        {
            rbTopTree.isKinematic = false;
            StartCoroutine(FadeOutAndDestroy());  // Bắt đầu Coroutine với fade-out rõ ràng
            PickedWood();
            isTopTree = false;
            TrunkTree.SetActive(true);  // Kích hoạt TrunkTree

        }
        else
        {
            isTrunkFading = true;
            TrunkTree.SetActive(false);
            PickedWood();
            Destroy(gameObject, 2f);
        }
    }

    private void PickedWood()
    {
        caculatedDropAmount();
        woodParticles.gameObject.SetActive(true);
        woodParticles.transform.parent = null;
        woodParticles.Play();

        // Gọi sự kiện để thông báo thu thập Wood
        onPickupWood?.Invoke(itemData.itemName, dropAmount);//InventoryManager

        Invoke("DisableParticles", 1f);
    }


    private void DisableParticles()
    {
        woodParticles.gameObject.SetActive(false);
    }

    private IEnumerator FadeOutAndDestroy()
    {

        isTopFading = true;
        timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;

            // Lerp giá trị alpha từ 1 xuống 0 (mờ dần)
            float alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);

            // Cập nhật màu sắc của vật thể để làm mờ (cập nhật alpha)
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            topTreeRenderer.material.color = newColor;

            yield return null; // Tiếp tục mỗi frame
        }

        // Đảm bảo vật thể hoàn toàn mờ trước khi hủy
        Color finalColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        topTreeRenderer.material.color = finalColor;

        // Sau khi fade-out hoàn tất, hủy vật thể
        Destroy(TopTree.gameObject);
    }

    public void TakeDamage(float target)
    {
        if (isTopTree)
        {
            Debug.Log("Dâmgeee");
            ShakeTree(); // Gọi rung cây
            TopTreeHealth -= target;
        }
        else
            TrunkTreeHealth -= target;

       

    }

    private void ShakeTree()
    {
        // Rung xoay quanh trục X và Z
        LeanTween.rotate(gameObject, new Vector3(0.01f, 0f, 0.01f), 0.1f).setLoopPingPong(1);
        // Xoay ±5 độ ở trục X và ±0.1 độ ở trục Z trong 0.2 giây, với 1 lần lặp lại
    }

}
