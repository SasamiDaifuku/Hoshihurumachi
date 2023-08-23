using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class SoundSettings : MonoBehaviour
{
    private SoundManager soundManager;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [SerializeField] private AudioClip seSettingAudio;


    /// <summary>
    /// 音量設定の表示/非表示の操作に使用
    /// </summary>
    [SerializeField] private CanvasGroup canvasGroupSound;
    // Start is called before the first frame update
    void Start()
    {
        GameObject objectSoundManager = CheckOtherSoundManager();
        soundManager = objectSoundManager.GetComponent<SoundManager>();
        bgmSlider.enabled = false;
        seSlider.enabled = false;
        //クリックイベントを購読
        soundButton.onClick.AsObservable()
            .Subscribe(_ => SoundButtonClickEvent())
            .AddTo(this);
        closeButton.onClick.AsObservable()
            .Subscribe(_ => CloseButtonClickEvent())
            .AddTo(this);
        //スライダーのイベントを購読
        bgmSlider.onValueChanged.AsObservable()
            .Subscribe(_ => soundManager.GetSetBgmVolume = bgmSlider.value)
                .AddTo(this);
        seSlider.onValueChanged.AsObservable()
                .Subscribe(_ => soundManager.GetSetSeVolume = seSlider.value)
                .AddTo(this);
            
    }

    private GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundManager");
    }

    private void SoundButtonClickEvent()
    {
        soundButton.interactable = false;
        stageManager.GetSetPlayState = StageManager.PlayState.Setting;

        bgmSlider.enabled = true;
        seSlider.enabled = true;
        bgmSlider.value = soundManager.GetSetBgmVolume;
        seSlider.value = soundManager.GetSetSeVolume;
        canvasGroupSound.blocksRaycasts = true;
        canvasGroupSound.DOFade(1.0f, 0.5f);
    }

    private void CloseButtonClickEvent()
    {
        soundButton.interactable = true;
        stageManager.GetSetPlayState = StageManager.PlayState.Ready;

        bgmSlider.enabled = false;
        seSlider.enabled = false;
        canvasGroupSound.blocksRaycasts = false;
        canvasGroupSound.DOFade(0.0f, 0.5f);
    }
    /// <summary>
    /// SEのスライダーの値変更時にSEを実行する
    /// </summary>
    public void SliderSeSettingPlay()
    {
        soundManager.PlaySe(seSettingAudio); 
    }
}
