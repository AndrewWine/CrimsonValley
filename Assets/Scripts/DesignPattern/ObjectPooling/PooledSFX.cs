using UnityEngine;
using System.Collections;

public class PooledSFX : MonoBehaviour
{
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip, float volume = 1f, float pitchMin = 0.9f, float pitchMax = 1.1f)
    {
        source.clip = clip;
        source.volume = volume;
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.Play();

        StartCoroutine(ReturnToPool(clip.length));
    }

    private IEnumerator ReturnToPool(float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.Instance.ReturnObject(gameObject);
    }
}
