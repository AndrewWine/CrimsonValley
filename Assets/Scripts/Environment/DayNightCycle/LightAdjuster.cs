using System;
using UnityEngine;

public class LightAdjuster : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Light sceneLight; // Đèn trong scene (kéo vào đây trong Inspector)
    [SerializeField] private DayNightCycle dayNightCycle; // DayNightCycle (kéo vào đây trong Inspector)
    private float currentSunIntensity = 1f; // Biến để lưu trữ độ sáng của mặt trời
    private float lightRange = 1000f; // Phạm vi chiếu sáng của đèn

    private void Start()
    {
        if (dayNightCycle == null || sceneLight == null)
        {
            Debug.LogError("DayNightCycle hoặc Scene Light chưa được thiết lập trong Inspector!");
            return;
        }

        // Đảm bảo rằng phạm vi ánh sáng là 30 đơn vị
        sceneLight.range = lightRange;
    }

    private void Update()
    {
        if (dayNightCycle != null && sceneLight != null)
        {
            // Lấy giá trị intensity từ DayNightCycle
            currentSunIntensity = dayNightCycle.GetSunIntensity();

            // Cập nhật ánh sáng của đèn dựa trên giá trị sunIntensity
            sceneLight.intensity = currentSunIntensity;

            // Nếu bạn muốn đèn sáng yếu hơn vào ban đêm và mạnh hơn khi mặt trời lên, có thể điều chỉnh giá trị range này theo độ sáng.
            // Ví dụ: Phạm vi ánh sáng cũng có thể thay đổi với độ sáng của mặt trời (sun intensity)
            sceneLight.range = Mathf.Lerp(15f, lightRange, currentSunIntensity);  // Điều chỉnh phạm vi ánh sáng (từ 15 đến 30 đơn vị)
        }
    }
}
