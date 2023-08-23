using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    //FadeCanvas取得
    [SerializeField]
    private Fade fade;

    //フェード時間(秒)
    [SerializeField]
    private float fadeTime;

    // Start is called before the first frame update
    void Start()
    {
        //シーン開始時にフェードをかける
        fade.FadeOut(fadeTime);
    }
    /// <summary>
    /// 各ボタンを押した時の処理
    /// </summary>
    /// <param name="sceneNum"></param>
    public void NextSceneTransition(int sceneNum)
    {
        //フェードをかけてからシーン遷移する
        fade.FadeIn(fadeTime, () =>
        {
            SceneManager.LoadScene(sceneNum);
        });
    }
    public void CurrentSceneTransition()
    {
        //フェードをかけてからシーン遷移する
        fade.FadeIn(fadeTime, () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
