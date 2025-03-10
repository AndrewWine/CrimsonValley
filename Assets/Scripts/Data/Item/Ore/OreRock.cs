using UnityEngine;
using System.Collections;

public class OreRock : MonoBehaviour, IDamageAble
{
    [Header("Elements")]
    public OreData oreData;
    public float health = 3; // Số lần đào trước khi vỡ
    [SerializeField]private ParticleSystem oreParticle;


    public void MineOre()
    {
        if (oreData != null)
        {
            InventoryManager.Instance.PickUpItemCallBack(oreData.ingotToGive.itemName, oreData.dropAmount);
        }

        // Tách hiệu ứng hạt ra khỏi quặng trước khi phá hủy
        if (oreParticle != null)
        {
            oreParticle.transform.SetParent(null); // Tách khỏi OreRock
            oreParticle.gameObject.SetActive(true); // Kích hoạt lại particle system
            oreParticle.Play();
            Destroy(oreParticle.gameObject, oreParticle.main.duration); // Xóa particle sau khi chạy xong
        }

        Destroy(gameObject);
    }


    private IEnumerator ShakeEffect()
    {
        Vector3 originalPos = transform.position;
        float elapsedTime = 0f;
        float duration = 0.1f;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-0.1f, 0.1f);
            float y = Random.Range(-0.1f, 0.1f);
            transform.position = originalPos + new Vector3(x, y, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("- HP ore");
        health -= damage;

        if (oreParticle != null && oreParticle.gameObject != null)
        {
            oreParticle.gameObject.SetActive(true); // Kích hoạt lại particle system
            oreParticle.Play();
        }
        else
        {
            Debug.LogWarning("oreParticle đã bị hủy!");
        }

        StartCoroutine(ShakeEffect());

        if (health <= 0)
        {
            MineOre();
        }

     }
    }
