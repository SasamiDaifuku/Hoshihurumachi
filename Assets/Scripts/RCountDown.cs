using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class RCountDown : MonoBehaviour
{
    
    //カウントダウン現在時刻
    private float _countDownNowTime = 0;
    
    // カウントダウン開始時間
    [SerializeField] private float countDownStartTime = 0;
    // カウントダウンテキスト
    [SerializeField] private TextMeshProUGUI countdownText;
    //開始時のSE
    private SoundManager _soundManager;
    [SerializeField] private AudioClip gameStartClip;
    
    public RCountDown()
    {
    }
    //カウントダウン完了時にtrueとする
    public bool IsComplete { get; private set; } = false;

    // Update is called once per frame
    private void Start()
    {
        countdownText.gameObject.SetActive(true);
        _countDownNowTime =  countDownStartTime;
        var objectSoundManager = CheckOtherSoundManager();
        _soundManager = objectSoundManager.GetComponent<SoundManager>();
        IsComplete = false;
        Debug.Log($"RCountDown :{IsComplete}");
    }

    private void Update()
    {
        //時間を引いていく
        _countDownNowTime -= Time.deltaTime;
        //カウントダウン中
        if (_countDownNowTime <= (float)countDownStartTime && _countDownNowTime > 0)
        {
            var intNum = (int)Mathf.Ceil(_countDownNowTime);
            countdownText.text = intNum.ToString();
        }
        else if (_countDownNowTime <= 0)
        {
            //開始
            _soundManager.PlaySe(gameStartClip);
            countdownText.text = "お散歩開始！！";
            // Start表示を少しして消す.
            StartCoroutine(WaitErase());
            IsComplete = true;
        }
    }
    
    /// <summary>
    /// 少し経過してからStart表示をけす
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitErase()
    {
        yield return new WaitForSeconds(0.5f);
        var seq = DOTween.Sequence();
        seq.Append(countdownText.DOFade(0, 0.5f));
        seq.Join(countdownText.DOScale(5, 0.5f))
            .AppendCallback(() => countdownText.gameObject.SetActive(false));
    }
    
    private static GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundManager");
    }
}
