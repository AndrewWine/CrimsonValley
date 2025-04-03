using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Elements")]
    public static AudioManager instance;
    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private Transform playerPos;
    public bool playBgm;
    private int bgmIndex;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else 
            instance = this;
    }

    private void Update()
    {
        if (!playBgm)
        {
            StopAllBGM();
        }
        else
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                PlayNextBGM();
            }
            else
            {
                return;
            }
        }
    }

    public void PlayNextBGM()
    {
        bgmIndex = (bgmIndex + 1) % bgm.Length; // Tăng index và reset nếu quá giới hạn
        PlayBGM(bgmIndex);
    }


    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        if (sfx[_sfxIndex].isPlaying)
            return;
        if (_source != null && Vector2.Distance(playerPos.position, _source.position) > sfxMinimumDistance)
            return;

        Debug.Log("Playe SFX");
        if(_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();


    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();
        bgm[bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for(int i = 0;i < bgm.Length;i++)
        {
            bgm[i].Stop();
        }
    }
}
