using System.Collections;
using UnityEngine;

public class Animals : MonoBehaviour
{
    [Header("Elements")]
    protected Animator animator;
    private AnimationClip[] animationClips;
    [Header("Random Animation Settings")]
    [SerializeField] protected float minDelay = 2f;
    [SerializeField] protected float maxDelay = 5f;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        animationClips = animator.runtimeAnimatorController.animationClips; // Lấy tất cả Animation Clips từ Animator Controller

        if (animationClips.Length > 0)
        {
            StartCoroutine(PlayRandomAnimation());
        }
    }

    private IEnumerator PlayRandomAnimation()
    {
        while (true) // Vòng lặp để liên tục phát animation ngẫu nhiên
        {
            // Chọn animation ngẫu nhiên từ tất cả các Animation Clips
            AnimationClip randomClip = animationClips[Random.Range(0, animationClips.Length)];

            // Phát animation từ clip ngẫu nhiên
            animator.Play(randomClip.name);

            // Chờ một khoảng thời gian ngẫu nhiên trước khi phát animation tiếp theo
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    protected virtual void OnStartAnimation()
    {
        if (animator != null && animationClips.Length > 0)
        {
            StopAllCoroutines(); // Đảm bảo chỉ chạy một coroutine duy nhất
            StartCoroutine(PlayRandomAnimation());
        }
    }

    protected virtual void OnStopAnimation()
    {
        if (animator != null)
        {
            animator.StopPlayback(); // Dừng playback animation hiện tại
            StopAllCoroutines(); // Dừng tất cả các coroutine
        }
    }
}

