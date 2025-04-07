using TMPro;
using UnityEngine;
using UnityEngine.UI;  // Đảm bảo bạn thêm namespace này để sử dụng UI Text

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float totalDayLength = 15f; // 15 phút cho 1 chu kỳ ngày-đêm
    [SerializeField, Range(0f, 1f)] public float timeOfDay;
    public int dayNumber = 0, yearNumber = 0, yearLength = 100;
    private float timeScale;
    public bool pause = false;

    [Header("Sun Settings")]
    [SerializeField] private Transform dailyRotation;
    [SerializeField] private Light sun;
    [SerializeField] private float sunBaseIntensity = 1f, sunVariation = 1.5f;
    [SerializeField] private Gradient sunColor;

    [Header("TimeChanger")]
    [SerializeField] private Material skybox;
    private float _elapsedTime = 0f;
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");
    private static readonly int Exposure = Shader.PropertyToID("_Exposure");

    [Header("Elements")]
    [SerializeField] private PlayerBlackBoard blackBoard;

    // UI Element to display time
    [SerializeField] private TextMeshProUGUI timeText;  // Thêm Text UI để hiển thị thời gian trong ngày
    [SerializeField] private TextMeshProUGUI dayAndYearText;  // Thêm Text UI để hiển thị thời gian trong ngày
    [SerializeField] private Image DayIcon;  // Thêm Text UI để hiển thị thời gian trong ngày
    [SerializeField] private Image NightIcon;  // Thêm Text UI để hiển thị thời gian trong ngày


    private void Start()
    {
        // Đặt thời gian mặc định vào sáng sớm
        timeOfDay = 0.245f; // Tương đương khoảng 21h (ban đêm)

        // Tính toán lại ánh sáng ngay từ đầu
        AdjustSun();
        AdjustSkybox();
    }

    private void OnEnable()
    {
        SleepState.NotifyIsSleep += SkipToNextDay;
    }

    private void OnDisable()
    {
        SleepState.NotifyIsSleep -= SkipToNextDay;
    }

    private void Update()
    {
        if (!pause)
        {
            timeScale = 1f / totalDayLength; // 1 đơn vị timeOfDay tương ứng với 15 phút
            UpdateTime();
        }
        AdjustSun();
        AdjustSkybox();

        // Cập nhật UI Time Text
        UpdateTimeText();
    }

    private void UpdateTime()
    {
        timeOfDay += Time.deltaTime * timeScale / 60f; // 60s trong 1 phút
        if (timeOfDay > 1f)
        {
            timeOfDay -= 1f;
            if (++dayNumber > yearLength)
            {
                dayNumber = 0;
                yearNumber++;
            }
        }
    }

    private void AdjustSun()
    {
        dailyRotation.localRotation = Quaternion.Euler(0f, 0f, timeOfDay * 360f);

        float intensity = Mathf.Clamp01(Vector3.Dot(sun.transform.forward, Vector3.down));

        // Điều chỉnh độ sáng theo tỷ lệ ngày/đêm
        if (timeOfDay < 0.6667f) // 2/3 thời gian là ban ngày
        {
            sun.intensity = Mathf.Lerp(sunBaseIntensity, sunVariation, intensity);
            blackBoard.timeToSleep = false;
            DayIcon.gameObject.SetActive(true);
            NightIcon.gameObject.SetActive(false);
        }
        else // 1/3 thời gian là ban đêm
        {
            sun.intensity = Mathf.Lerp(sunBaseIntensity * 0.2f, sunBaseIntensity, intensity);
            blackBoard.timeToSleep = true;
            DayIcon.gameObject.SetActive(false);
            NightIcon.gameObject.SetActive(true);
        }
        sun.color = sunColor.Evaluate(intensity);
    }

    private void AdjustSkybox()
    {
        // Điều chỉnh rotation của skybox theo thời gian trong ngày
        float skyRotation = timeOfDay * 360f;
        skybox.SetFloat(Rotation, skyRotation);

        // Điều chỉnh độ sáng của Skybox dựa trên thời gian trong ngày
        float sunIntensity = Mathf.Clamp01(Vector3.Dot(sun.transform.forward, Vector3.down));

        // Nếu trời sáng, đặt Exposure cao hơn, nếu trời tối thì thấp hơn
        float exposureValue = Mathf.Lerp(0.3f, 1.2f, sunIntensity);
        skybox.SetFloat(Exposure, exposureValue);
    }

    public float GetSunIntensity()
    {
        return sun.intensity;
    }

    public float GetCurrentHour()
    {
        return timeOfDay;
    }

    public void SkipToNextDay()
    {
        dayNumber++; // Tăng ngày lên 1
        timeOfDay = 0.25f; // Đặt thời gian về 6h sáng

        // Cập nhật lại ánh sáng và skybox ngay lập tức
        AdjustSun();
        AdjustSkybox();
    }

    // Phương thức tính toán thời gian theo giờ và phút
    private void UpdateTimeText()
    {
        int hour = Mathf.FloorToInt(timeOfDay * 24f);  // Chuyển đổi timeOfDay thành giờ (0-23)
        int minute = Mathf.FloorToInt((timeOfDay * 1440f) % 60);  // Chuyển đổi timeOfDay thành phút (0-59)

        // Cập nhật thời gian hiển thị trên UI
        timeText.text = $"{hour:00}:{minute:00}";
        dayAndYearText.text = $"Day: {dayNumber}\nYear: {yearLength}";
    }
}
