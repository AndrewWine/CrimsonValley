using System.Collections;
using UnityEngine;

public class Animals : MonoBehaviour
{
    [Header("Elements")]
    protected Animator animator;

    [Header("Random Animation Settings")]
    [SerializeField] protected string[] animations; // 🛠 Chuyển thành mảng
    [SerializeField] protected float minDelay = 5f;
    [SerializeField] protected float maxDelay = 10f;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PlayRandomAnimation());
    }


    private IEnumerator PlayRandomAnimation()
    {
        while (true)
        {
            float randomDelay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(randomDelay);

            if (animations.Length > 0)
            {
                string randomAnimation = animations[Random.Range(0, animations.Length)];
                animator.Play(randomAnimation);
            }
        }
    }

    protected virtual void OnStartAnimation()
    {
        if (animator != null && animations.Length > 0)
        {
            StopAllCoroutines(); // 🔹 Đảm bảo chỉ chạy 1 coroutine duy nhất
            StartCoroutine(PlayRandomAnimation());
        }
    }

    protected virtual void OnStopAnimation()
    {
        if (animator != null)
        {
            animator.StopPlayback();
            StopAllCoroutines();
            StartCoroutine(PlayRandomAnimation());
        }
    }
}
