using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private FadeManager fadeManager;
    public int seneNum;

    /// <summary>
    /// ゲームステート
    /// </summary>
    public enum PlayState
    {
        Ready,
        Setting,
    }
    // 現在のステート
    private PlayState currentState = PlayState.Ready;
    public PlayState GetSetPlayState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    // Update is called once per frame
    void Update()
    {
        //ボタンがクリックされたときは画面クリックを無視する
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (currentState == PlayState.Ready)
        {
            //画面がクリックされたときはシーン遷移
            if (Input.GetMouseButtonDown(0))
            {
                fadeManager.NextSceneTransition(seneNum);
            }
        }
        
    }
}
