using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;
using UnityEngine.Serialization;
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

    public PlayState GetSetPlayState { get; private set; } = PlayState.None;

    //! カウントダウンスタートタイム
    [SerializeField] private int countStartTime = 3;

    //! カウントダウンテキスト
    [SerializeField] private TextMeshProUGUI countdownText = null;

    // カウントダウンの現在値
    private float _currentCountDown = 0;

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
    [FormerlySerializedAs("sceneTitleNum")] public int sceneTitleNum;

    /// <summary>
    /// ゲーム画面のシーン番号
    /// </summary>
    [FormerlySerializedAs("sceneGameNum")] public int sceneGameNum;

    [SerializeField] private Button retryButton;
    [SerializeField] private Button endButton;
    [SerializeField] private Button tweetButton;
    [SerializeField] private TimeManager timeManager;

    private SoundManager _soundManager;
    [SerializeField] private AudioClip gameStartClip;

    private void Start()
    {
        retryButton.enabled = false;
        endButton.enabled = false;
        //クリックイベントを購読
        endButton.onClick.AsObservable()
            .Subscribe(_ => fadeManager.NextSceneTransition(sceneTitleNum))
            .AddTo(this);
        retryButton.onClick.AsObservable()
            .Subscribe(_ => fadeManager.NextSceneTransition(sceneGameNum))
            .AddTo(this);

        GameObject objectSoundManager = CheckOtherSoundManager();
        _soundManager = objectSoundManager.GetComponent<SoundManager>();
        //カウントダウンタイマーをスタート
        CountDownStart();
    }

    private void Update()
    {
        //ステートがReadyの時カウントダウン
        if (GetSetPlayState != PlayState.Ready) return;
        //時間を引いていく
        _currentCountDown -= Time.deltaTime;
        //カウントダウン中
        if (_currentCountDown <= (float)countStartTime && _currentCountDown > 0)
        {
            var intNum = (int)Mathf.Ceil(_currentCountDown);
            countdownText.text = intNum.ToString();
        }
        else if (_currentCountDown <= 0)
        {
            //開始
            GetSetPlayState = PlayState.Play;
            _soundManager.PlaySe(gameStartClip);
            countdownText.text = "お散歩開始！！";
            // Start表示を少しして消す.
            StartCoroutine(WaitErase());
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

    /// <summary>
    /// ゲームオーバー表示
    /// </summary>
    public void DisplayGameOver()
    {
        GetSetPlayState = PlayState.End;
        resultText.text = "ねこちゃんがフキゲンに\nなっちゃった！";
        // スコア取得処理
        var scoreTime = timeManager.GetSetTime;
        scoreText.text = $"結果{scoreTime:0.00}秒";
        //クリックイベントを購読
        tweetButton.onClick.AsObservable()
            .Subscribe(_ => naichilab.UnityRoomTweet.Tweet("hoshifuru_machino_nekochan",
                $"ねこちゃんとのお散歩を{scoreTime:0.00}秒楽しみました！！", "星降る町のねこちゃん", "unity1week"))
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
    private void CountDownStart()
    {
        _currentCountDown = (float)countStartTime;
        GetSetPlayState = PlayState.Ready;
        countdownText.gameObject.SetActive(true);
    }

    private static GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundManager");
    }
}