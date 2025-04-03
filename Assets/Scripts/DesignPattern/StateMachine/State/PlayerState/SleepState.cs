using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SleepState : PlayerState
{
    [Header("Elements")]
    [SerializeField] private Transform sleepSpawn; // Điểm dịch chuyển khi ngủ dậy
    [SerializeField] private Image fadeScreen; // Panel UI màu đen
    [SerializeField] private GameObject playerObject; // Kéo thả GameObject của Player
    private Rigidbody rb;
    private CharacterController controller;


    public static Action NotifyIsSleep;

    public override void Enter()
    {
        base.Enter();
        blackboard.sleepButtonPressed = false;
        NotifyIsSleep?.Invoke();

        if (playerObject == null)
        {
            Debug.LogError(" Không tìm thấy GameObject của Player! Hãy kéo thả vào SleepState.");
            return;
        }

        rb = playerObject.GetComponent<Rigidbody>();
        controller = playerObject.GetComponent<CharacterController>();
        StartCoroutine(SleepSequence());
    }

    private IEnumerator SleepSequence()
    {
        fadeScreen.gameObject.SetActive(true);

        yield return StartCoroutine(FadeToBlack(1f));
        yield return new WaitForSeconds(1f);

        // 🔹 Ngắt hệ thống di chuyển trước khi dịch chuyển nhân vật
        if (rb != null)
        {
            rb.isKinematic = true; // Vô hiệu hóa physics để không bị kéo về vị trí cũ
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        if (controller != null)
        {
            controller.enabled = false; // Tắt CharacterController
        }

        //  Dịch chuyển nhân vật đến sleepSpawn
        playerObject.transform.position = sleepSpawn.position;

        yield return StartCoroutine(FadeFromBlack(1f));

        fadeScreen.gameObject.SetActive(false);

        //  Bật lại hệ thống di chuyển
        if (rb != null) rb.isKinematic = false;
        if (controller != null) controller.enabled = true;
        PlayerStatusManager.Instance.ResetStamina(100); 
        EventBus.Publish(new StaminaChangedEvent(blackboard.stamina));
        
        stateMachine.ChangeState(blackboard.idlePlayer);
    }

    private IEnumerator FadeToBlack(float duration)
    {
        fadeScreen.color = new Color(0f, 0f, 0f, 0f);
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
