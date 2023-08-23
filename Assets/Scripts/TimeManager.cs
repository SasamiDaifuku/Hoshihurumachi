using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TimeManager : MonoBehaviour
{
    /// <summary>
    /// タイマーの設定値
    /// </summary>
    [Header("タイマーの設定値")]
    [SerializeField] private int settingTime;

    /// <summary>
    /// タイマーの時間
    /// </summary>
    private float currentTime;

    /// <summary>
    /// 画面上のタイマー表示
    /// </summary>
    [SerializeField] private TextMeshProUGUI displayTimerText;

    [SerializeField] private GameController gameController;

    public float GetSetTime
    {
        get { return currentTime; }
        private set { currentTime = value; }
    }
    /// <summary>
    /// 計測用タイマー
    /// </summary>
    private float measureTimer;
    // Start is called before the first frame update
    void Start()
    {
        //現在の残り時間にタイマーの設定値を入れる
        currentTime = settingTime;
        //バトル時間の表示を更新
        UpdateDisplayBattleTime(currentTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.GetSetPlayState == GameController.PlayState.Play)
        {
            //バトル時間の表示を更新
            currentTime += Time.deltaTime;
            UpdateDisplayBattleTime(currentTime);
        }
        
    }

    /// <summary>
    /// バトル時間の表示を更新
    /// </summary>
    /// <param name="currentTime"></param>
    private void UpdateDisplayBattleTime(float currentTime)
    {
        //バトルの残り時間を更新
        //displayTimerText.text = currentTime.ToString();
        displayTimerText.text = "TIME: " + currentTime.ToString("0.00");
    }
}
