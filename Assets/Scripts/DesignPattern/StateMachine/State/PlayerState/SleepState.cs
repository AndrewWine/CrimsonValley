using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SleepState : PlayerState
{
    [Header("Elements")]
    [SerializeField] private Transform bedPosition;
    [SerializeField] private Image fadeScreen; // Panel UI màu đen

    public static Action NotifyIsSleep;

    public override void Enter()
    {
        base.Enter();
        blackboard.sleepButtonPressed = false;
        transform.position = bedPosition.position;
        NotifyIsSleep?.Invoke();

        StartCoroutine(SleepSequence()); // Bắt đầu hiệu ứng ngủ
        stateMachine.ChangeState(blackboard.idlePlayer);

    }

    private IEnumerator SleepSequence()
    {
        fadeScreen.gameObject.SetActive(true); // Bật UI Panel nhưng vẫn trong suốt

        yield return StartCoroutine(FadeToBlack(1f)); // Màn hình tối dần trong 1 giây
        yield return new WaitForSeconds(1f); // Giữ màn hình tối trong 1 giây
        yield return StartCoroutine(FadeFromBlack(1f)); // Màn hình sáng dần trong 1 giây

        fadeScreen.gameObject.SetActive(false); // Tắt UI Panel khi hoàn tất

    }


    private IEnumerator FadeToBlack(float duration)
    {
        fadeScreen.color = new Color(0f, 0f, 0f, 0f); // Đảm bảo Alpha ban đầu = 0
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / duration);
            fadeScreen.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
    }

    private IEnumerator FadeFromBlack(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / duration);
            fadeScreen.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

    }
}
