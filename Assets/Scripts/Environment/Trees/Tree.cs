using System;
using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour, IDamageAble
{
    [Header("Elements")]
    [SerializeField] private GameObject topTree;
    [SerializeField] private GameObject trunkTree;
    [SerializeField] private ParticleSystem woodParticles;
    [SerializeField] private Rigidbody rbTopTree;
    [SerializeField] private Collider trunkCollider;
    public ItemData itemData;
    [Header("Settings")]
    [SerializeField] private float topTreeHealth = 10f;
    [SerializeField] private float trunkTreeHealth = 10f;
    [SerializeField] private int dropAmount = 3;

    [Header("Actions")]
    public static Action reduceDurability;

    private InventoryManager inventoryManager;
    private bool isTopTree = true;
    private bool isPlayingParticles = false;

    private void Start()
    {
        
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError(" Không tìm thấy InventoryManager!");
        }

        InitializeTree();
    }

    private void Update()
    {
        if (isTopTree && topTreeHealth <= 0)
        {
            ExecuteTopTree();
        }
        else if (!isTopTree && trunkTreeHealth <= 0)
        {
            ExecuteTrunkTree();
        }
    }

    private void InitializeTree()
    {
        topTree.SetActive(true);
        trunkTree.SetActive(true);
        rbTopTree.isKinematic = true;
        trunkCollider.isTrigger = true;
        woodParticles.gameObject.SetActive(false);
    }

    private void ExecuteTopTree()
    {
        AudioManager.instance.PlaySFX(6,null);
        PlayParticles();
        rbTopTree.isKinematic = false;
        rbTopTree.AddForce(Vector3.right * 0.5f, ForceMode.Impulse);
        DropWood();
        isTopTree = false;
        StartCoroutine(EnableTrunkPhysics());

        Destroy(topTree, 3.5f);
    }

    private IEnumerator EnableTrunkPhysics()
    {
        yield return new WaitForSeconds(1f);
        trunkCollider.isTrigger = false;
    }

    private void ExecuteTrunkTree()
    {
        PlayParticles();
        trunkTree.SetActive(false);
        DropWood();

        // Gửi sự kiện cây bị phá hủy
        EventBus.Publish(new TreeDestroyedEvent(transform.position));

        Destroy(gameObject);
    }


    private void DropWood()
    {
        woodParticles.transform.parent = null;
        inventoryManager?.PickUpItemCallBack(itemData.itemName, dropAmount);
    }

    private void PlayParticles()
    {
        if (!isPlayingParticles)
        {
            isPlayingParticles = true;
            woodParticles.gameObject.SetActive(true);
            woodParticles.Play();
            Invoke(nameof(DisableParticles), 1f);
        }
    }

    private void DisableParticles()
    {
        woodParticles.gameObject.SetActive(false);
        isPlayingParticles = false;
    }

    public void TakeDamage(float damage)
    {
        reduceDurability?.Invoke();
        if (isTopTree)
        {
            ShakeTree();
            topTreeHealth -= damage;
        }
        else
        {
            trunkTreeHealth -= damage;
        }
    }

    private void ShakeTree()
    {
        LeanTween.rotate(gameObject, new Vector3(0.1f, 0f, 0.05f), 0.0001f).setLoopPingPong(1);
    }
}
