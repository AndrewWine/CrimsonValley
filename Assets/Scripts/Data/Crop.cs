using UnityEngine;
using System;
public class Crop : Item
{
    [Header("Elements")]
    [SerializeField] private Transform cropRenderer;
    [SerializeField] private ParticleSystem harvestedParticles;

    private int TimeToGrowUp = 10;
    private int TimeToGrowDown = 1;

    private bool isFullyGrown = false; //Biến kiểm soát

    [Header("Actions")]
    public static Action realdyToHarvest;
    public static Action arealdyHarvested;

    public void ScaleUp()
    {
        LeanTween.scale(cropRenderer.gameObject, cropRenderer.transform.localScale * 10f, TimeToGrowUp)
            .setEase(LeanTweenType.easeOutBack)
            .setOnComplete(() =>
            {
                isFullyGrown = true; //Cây đã phát triển xong
                realdyToHarvest?.Invoke(); // Gửi sự kiện cây đã sẵn sàng thu hoạch
            });
    }

    public bool IsFullyGrown()
    {
        return isFullyGrown; // Trả về trạng thái cây
    }

    public void ScaleDown()
    {
        if (!isFullyGrown) return; // Nếu chưa lớn đủ, không thu hoạch

        Destroy(gameObject); // Xóa cây sau khi hoàn thành
        Debug.Log("Da scaledown");

        // Kích hoạt hiệu ứng hạt sau khi thu hoạch
        harvestedParticles.gameObject.SetActive(true);
        harvestedParticles.transform.parent = null;
        harvestedParticles.Play();

        Debug.Log("Da Play");
    }
}
