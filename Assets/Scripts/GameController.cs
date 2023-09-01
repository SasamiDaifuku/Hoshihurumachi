using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;
using unityroom.Api;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// ゲームステート
    /// </summary>
    public enum PlayState
    {
        None,
        Ready,
        Play,
        End,
    }

    // 現在のステート
    private PlayState currentState = PlayState.None;

    public PlayState GetSetPlayState
    {
        get { return currentState; }
        private set { currentState = value; }
    }

    //! カウントダウンスタートタイム
    [SerializeField] private int countStartTime = 3;

    //! カウントダウンテキスト
    [SerializeField] private TextMeshProUGUI countdownText = null;

    // カウントダウンの現在値
    float currentCountDown = 0;

    /// <summary>
    /// ゲーム終了時に表示するテキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI resultText;

    /// <summary>
    /// ゲーム終了時に表示するスコアテキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI scoreText;

    /// <summary>
    /// ゲーム終了時の表示/非表示の操作に使用
    /// </summary>
    [SerializeField] private CanvasGroup canvasGroupGameEnd;

    /// <summary>
    /// フェードに使用
    /// </summary>
    [SerializeField] private FadeManager fadeManager;

    /// <summary>
    /// タイトルのシーン番号
    /// </summary>
    public int seneTitleNum;

    /// <summary>
    /// ゲーム画面のシーン番号
    /// </summary>
    public int seneGameNum;

    [SerializeField] private Button retryButton;
    [SerializeField] private Button endButton;
    [SerializeField] private Button tweetButton;
    [SerializeField] private TimeManager timeManager;

    private SoundManager soundManager;
    [SerializeField] private AudioClip gameStartClip;

    private void Start()
    {
        retryButton.enabled = false;
        endButton.enabled = false;
        //クリックイベントを購読
        endButton.onClick.AsObservable()
            .Subscribe(_ => fadeManager.NextSceneTransition(seneTitleNum))
            .AddTo(this);
        retryButton.onClick.AsObservable()
            .Subscribe(_ => fadeManager.NextSceneTransition(seneGameNum))
            .AddTo(this);

        GameObject objectSoundManager = CheckOtherSoundManager();
        soundManager = objectSoundManager.GetComponent<SoundManager>();
        //カウントダウンタイマーをスタート
        CountDownStart();
    }

    private void Update()
    {
        //ステートがReadyの時カウントダウン
        if (currentState == PlayState.Ready)
        {
            //時間を引いていく
            currentCountDown -= Time.deltaTime;
            int intNum = 0;
            //カウントダウン中
            if (currentCountDown <= (float)countStartTime && currentCountDown > 0)
            {
                intNum = (int)Mathf.Ceil(currentCountDown);
                countdownText.text = intNum.ToString();
            }
            else if (currentCountDown <= 0)
            {
                //開始
                GetSetPlayState = PlayState.Play;
                intNum = 0;
                soundManager.PlaySe(gameStartClip);
                countdownText.text = "お散歩開始！！";
                // Start表示を少しして消す.
                StartCoroutine(WaitErase());
            }
        }
        /*
        //デバッグ用
        if (Input.GetMouseButtonDown(0))
        {
            DisplayGameOver();
        }
        */
    }

    /// <summary>
    /// 少し経過してからStart表示をけす
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitErase()
    {
        yield return new WaitForSeconds(0.5f);
        Sequence seq = DOTween.Sequence();
        seq.Append(countdownText.DOFade(0, 0.5f));
        seq.Join(countdownText.DOScale(5, 0.5f))
            .AppendCallback(() => countdownText.gameObject.SetActive(false));

        //countdownText.gameObject.SetActive(false);
    }

    /// <summary>
    /// ゲームオーバー表示
    /// </summary>
    public void DisplayGameOver()
    {
        GetSetPlayState = PlayState.End;
        resultText.text = "ねこちゃんがフキゲンに\nなっちゃった！";
        // スコア取得処理
        float scoreTime = timeManager.GetSetTime;
        scoreText.text = string.Format("結果{0}秒", scoreTime.ToString("0.00"));
        //クリックイベントを購読
        tweetButton.onClick.AsObservable()
            .Subscribe(_ => naichilab.UnityRoomTweet.Tweet("hoshifuru_machino_nekochan",
                string.Format("ねこちゃんとのお散歩を{0}秒楽しみました！！", scoreTime.ToString("0.00")), "星降る町のねこちゃん", "unity1week"))
            .AddTo(this);
        DisplayCanvas();
        // ボードNo1にスコアを送信する。
        UnityroomApiClient.Instance.SendScore(1, scoreTime, ScoreboardWriteMode.Always);
    }

    /// <summary>
    /// キャンバスグループの表示
    /// </summary>
    private void DisplayCanvas()
    {
        retryButton.enabled = true;
        endButton.enabled = true;
        canvasGroupGameEnd.DOFade(1.0f, 0.5f);
    }

    /// <summary>
    /// カウントダウンスタート
    /// </summary>
    void CountDownStart()
    {
        currentCountDown = (float)countStartTime;
        GetSetPlayState = PlayState.Ready;
        countdownText.gameObject.SetActive(true);
    }

    private GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundManager");
    }
}