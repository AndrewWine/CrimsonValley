using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private Slider staminaSlider; // Thanh hiển thị Stamina
    [SerializeField] private TextMeshProUGUI staminaText; // Text để hiển thị Stamina
    [SerializeField] private Gradient sliderColorGradient; // Gradient để thay đổi màu sắc

    private Coroutine staminaCoroutine;

    private void OnEnable()
    {
        EventBus.Subscribe<StaminaChangedEvent>(OnStaminaChanged);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<StaminaChangedEvent>(OnStaminaChanged);
    }

    private void Start()
    {
        staminaSlider.maxValue = 100;  // Đảm bảo giá trị tối đa là 100
        staminaSlider.value = 100;     // Đặt thanh đầy đủ (100 Stamina)
        UpdateStaminaText(100);        // Bắt đầu với Stamina đầy (100)
        staminaSlider.fillRect.GetComponent<Image>().color = sliderColorGradient.Evaluate(1f);  // Màu ban đầu (xanh)
    }

    private void OnStaminaChanged(StaminaChangedEvent e)
    {
        // Nếu có Coroutine đang chạy, hủy nó
        if (staminaCoroutine != null)
        {
            StopCoroutine(staminaCoroutine);
        }

        // Chạy hiệu ứng từ từ
        staminaCoroutine = StartCoroutine(UpdateStaminaOverTime(e.NewStamina));
    }

    private IEnumerator UpdateStaminaOverTime(float targetStamina)
    {
        float startStamina = staminaSlider.value;
        float elapsedTime = 0f;
        float duration = 1f; // Thời gian chuyển đổi (giây)

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            staminaSlider.value = Mathf.Lerp(startStamina, targetStamina, elapsedTime / duration);
            UpdateStaminaText(Mathf.Lerp(startStamina, targetStamina, elapsedTime / duration));

            // Cập nhật màu của thanh Slider theo giá trị Stamina
            float sliderValue = staminaSlider.value / 100f;
            staminaSlider.fillRect.GetComponent<Image>().color = sliderColorGradient.Evaluate(sliderValue);

            yield return null;
        }

        // Đảm bảo giá trị cuối cùng chính xác
        staminaSlider.value = targetStamina;
        UpdateStaminaText(targetStamina);
        staminaSlider.fillRect.GetComponent<Image>().color = sliderColorGradient.Evaluate(targetStamina / 100f);
    }

    // Cập nhật số Stamina trên Text UI
    private void UpdateStaminaText(float stamina)
    {
        staminaText.text = $"{stamina:F0}";  // Hiển thị số Stamina (F0 để không có số thập phân)
    }
}
