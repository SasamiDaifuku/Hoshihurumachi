using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource seAudioSource;

    /// <summary>
    /// BGMボリュームの設定
    /// </summary>
    public float GetSetBgmVolume
    {
        get { return bgmAudioSource.volume; }
        set { bgmAudioSource.volume = Mathf.Clamp01(value); }
    }
    /// <summary>
    /// SEボリュームの設定
    /// </summary>
    public float GetSetSeVolume
    {
        get { return seAudioSource.volume; }
        set { seAudioSource.volume = Mathf.Clamp01(value); }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameObject soundManager = CheckOtherSoundManager();
        bool isCheckResult = soundManager != null && soundManager != gameObject;
        if (isCheckResult)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 他にSoundManagerが存在するかチェック
    /// </summary>
    /// <returns></returns>
    private GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundManager");
    }

    /// <summary>
    /// BGMを流す
    /// </summary>
    /// <param name="clip"></param>
    public void PlayBgm(AudioClip clip)
    {
        bgmAudioSource.clip = clip;
        if(clip == null)
        {
            return;
        }
        bgmAudioSource.Play();
    }
    /// <summary>
    /// SEを流す
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySe(AudioClip clip)
    {
        if(clip == null)
        {
            return;
        }
        seAudioSource.PlayOneShot(clip);
    }


}
