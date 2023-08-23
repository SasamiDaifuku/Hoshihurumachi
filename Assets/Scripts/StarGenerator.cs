using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : MonoBehaviour
{
    [SerializeField] private StarCharacter starPrefab;
    [SerializeField] TimeManager timeManager;

    /// <summary>
    /// CanvasのTransformを登録
    /// </summary>
    [SerializeField] private Transform canvasTran;
    /// <summary>
    /// ゲーム画面の左端の指定用
    /// </summary>
    [SerializeField] private Transform leftTran;
    /// <summary>
    /// ゲーム画面の右端の指定用
    /// </summary>
    [SerializeField] private Transform rightTran;
    /// <summary>
    /// 生成した星を入れるフォルダの役割
    /// </summary>
    [SerializeField] private Transform starPlace;
    [SerializeField] private GameController gameController;

    //経過時間
    private float time = 0f;
    //星生成時間間隔
    private float intervalTime = 1f;

    enum GameLevel
    {
        LEVEL1,
        LEVEL2,
        LEVEL3,
        LEVEL4,
        LEVEL5,
    }

    private void Update()
    {
        if (gameController.GetSetPlayState == GameController.PlayState.Play)
        {
            //時間計測
            time += Time.deltaTime;
            //経過時間が生成時間になった時
            if (time > intervalTime)
            {
                //現在のレベルを決定する
                GameLevel gameLevel = GetLevel();
                //星の生成個数を決定する
                GenerateMultiStart(5, 10);
                //経過時間を初期化して再度時間計測を始める
                time = 0f;

                //時間間隔を決定する
                intervalTime = GetIntervalTime(gameLevel);
            }
        }
    }
    /// <summary>
    /// 時間間隔を決定する
    /// </summary>
    /// <param name="gameLevel"></param>
    /// <returns></returns>
    private float GetIntervalTime(GameLevel gameLevel)
    {
        switch (gameLevel)
        {
            case GameLevel.LEVEL1:
                return 1f;
            case GameLevel.LEVEL2:
                return 0.8f;
            case GameLevel.LEVEL3:
                return 0.6f;
            case GameLevel.LEVEL4:
                return 0.4f;
            case GameLevel.LEVEL5:
                return 0.2f;
            default:
                return 1f;
        }

    }
    /// <summary>
    /// レベルを決定する
    /// </summary>
    /// <returns></returns>
    private GameLevel GetLevel()
    {
        //経過時間を取得
        float currentTime = timeManager.GetSetTime;
        if (0 <= currentTime && currentTime <= 10)
        {
            //10秒以下
            return GameLevel.LEVEL1;
        }
        else if (10 < currentTime && currentTime <= 20)
        {
            //20秒以下
            return GameLevel.LEVEL2;
        }
        else if (20 < currentTime && currentTime <= 30)
        {
            //30秒以下
            return GameLevel.LEVEL3;
        }
        else if (30 < currentTime && currentTime <= 40)
        {
            //40秒以下
            return GameLevel.LEVEL4;
        }
        else
        {
            //40秒より大きい時
            return GameLevel.LEVEL5;
        }
    }
    /// <summary>
    /// 星を生成
    /// </summary>
    /// <param name="minInt"></param>
    /// <param name="maxInt"></param>
    /// <returns></returns>
    private void GenerateMultiStart(int minInt, int maxInt)
    {
        //生成する星の数をランダムで設定
        //int appearStarCount = Random.Range(minInt, maxInt);
        int appearStarCount = 1;
        //生成した数をカウント用
        int count = 0;
        //生成した数が生成数になるまでループする。目標数になったら生成終了しループを抜ける
        while (count < appearStarCount)
        {
            // Transform情報を代入
            Transform starTran = canvasTran;
            // 位置を画面内に収まる範囲でランダムに設定
            starTran.position = new Vector2(Random.Range(leftTran.localPosition.x, rightTran.localPosition.x), Random.Range(leftTran.localPosition.y, rightTran.localPosition.y));
            // 設定した位置に対してRayを発射
            RaycastHit2D hit = Physics2D.Raycast(starTran.position, Vector3.forward);

            // もしも何もRayに当たらない場合(Rayに何か当たった場合にはその位置には生成しないので、ループの最初からやり直す)
            if (hit.collider == null)
            {

                // 敵を生成
                StarCharacter starCharacter = Instantiate(starPrefab, canvasTran, false);

                // 親子関係を設定
                starCharacter.transform.SetParent(starPlace);

                // 敵の位置をランダムで設定した位置に設定
                starCharacter.transform.localPosition = starTran.position;

                // 敵の初期設定
                starCharacter.SetUpStarCharacter(canvasTran);

                // 敵の生成カウントを加算
                count++;
            }
        }

    }
}
