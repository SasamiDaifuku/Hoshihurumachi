using System.Collections;
using Cysharp.Threading.Tasks;
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
    public int sceneTitleNum;

    /// <summary>
    /// ゲーム画面のシーン番号
    /// </summary>
    public int sceneGameNum;

    [SerializeField] private Button retryButton;
    [SerializeField] private Button endButton;
    [SerializeField] private Button tweetButton;

    [SerializeField] private TimeManager timeManager;

    //カウントダウン用のオブジェクト
    [SerializeField] private GameObject rCountDownParent;
    [SerializeField] private RCountDown rCountDown;
    [SerializeField] private Transform rCountDownTransform;

    private async void Start()
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
        //ゲーム状態をReadyに変更
        GetSetPlayState = PlayState.Ready;
        //ゲーム開始までのカウントダウン
        await RCountDownStart();
        //カウントダウン完了時、ここの処理が呼ばれる
        //ゲーム状態をPlayに変更
        GetSetPlayState = PlayState.Play;
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
    /// ゲームスタート時のカウントダウンを表示
    /// </summary>
    /// <returns></returns>
    private async UniTask RCountDownStart()
    {
        //カウントダウンプレハブを生成
        var obj = Instantiate(rCountDown, rCountDownTransform.position, rCountDownTransform.rotation,
            rCountDownParent.transform);
        //カウントダウンタイマーをスタート
        while (!obj.IsComplete)
        {
            await UniTask.Yield();
        }
    }
}