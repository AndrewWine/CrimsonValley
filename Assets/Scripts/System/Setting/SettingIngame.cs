using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class SettingIngame : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject SettingWindow;
    [SerializeField] private GameObject SettingRectTransform;
    [SerializeField] private GameObject openButton;
    [SerializeField] private GameObject closeButton;

    private Vector3 hiddenPosition;  // Vị trí khi cửa sổ ở ngoài màn hình
    private Vector3 visiblePosition; // Vị trí khi cửa sổ mở (di chuyển vào trong)
    [SerializeField] private float moveSpeed = 2000f;  // Tăng tốc độ để tránh bị chậm
    [SerializeField] private float moveDistance = 200f; // Khoảng cách di chuyển


    private void Start()
    {
        InitialSetting();
    }

    private void InitialSetting()
    {
        if (SettingRectTransform == null) return;
        hiddenPosition = SettingRectTransform.transform.localPosition;  // Vị trí ban đầu (ngoài màn hình)
        visiblePosition = hiddenPosition + new Vector3(moveDistance, 0, 0);  // Di chuyển vào 200px
        openButton.SetActive(true);
        closeButton.SetActive(false);
    }

    public void OnOpenWindowPressed()
    {
        SettingWindow.SetActive(true);
        StartCoroutine(MoveIn(visiblePosition));

        openButton.SetActive(false);
        closeButton.SetActive(true);
    }

    public void OnCloseWindowPressed()
    {
        TooltipManager.Instance.HideTooltip();
        StartCoroutine(MoveOut(hiddenPosition));  // Di chuyển về vị trí cũ

        openButton.SetActive(true);
        closeButton.SetActive(false);
    }

    private IEnumerator MoveIn(Vector3 targetPosition)
    {
        while (Vector3.Distance(SettingRectTransform.transform.localPosition, targetPosition) > 1f)
        {
            SettingRectTransform.transform.localPosition = Vector3.MoveTowards(
                SettingRectTransform.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Đảm bảo vị trí chính xác khi kết thúc
        SettingRectTransform.transform.localPosition = targetPosition;
    }

    private IEnumerator MoveOut(Vector3 targetPosition)
    {
        while (Vector3.Distance(SettingRectTransform.transform.localPosition, targetPosition) > 1f) // Sửa điều kiện
        {
            SettingRectTransform.transform.localPosition = Vector3.MoveTowards(
                SettingRectTransform.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        // Đảm bảo vị trí chính xác khi kết thúc
        SettingRectTransform.transform.localPosition = targetPosition;
    }

    public void OnSaveGamePressed()
    {
        WorldManager.instance.SaveWorld();
        Debug.Log("Da bam nut save game");
    }

    public void ExitToMainMenu()
    {
        WorldManager.instance.SaveWorld();

        // Chuyển sang scene game trước khi load dữ liệu
        SceneManager.LoadScene("MainMenu");

    }
}
