using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CpuCat : MonoBehaviour
{
    /// <summary>
    /// 動かすオブジェクトを指定
    /// </summary>
    [SerializeField] private GameObject gameObjectCpuCat;
    /// <summary>
    /// 動くスピードを指定
    /// </summary>
    [SerializeField] private int speed = 5;
    /// <summary>
    /// 移動先を指定
    /// </summary>
    Vector2 movePosition;
    /// <summary>
    /// ゲーム終了時に呼び出す
    /// </summary>
    [SerializeField] private GameController gameController;
    private SoundManager soundManager;
    [SerializeField] private AudioClip[] catVoiceClip;
    [SerializeField] private AudioClip catDamageClip;

    void Start()
    {
        movePosition = actionMoveRandomPosition();
        //イベントトリガー
        var eventTrigger = this.gameObject.AddComponent<ObservableEventTrigger>();
        // PointerDownを
        eventTrigger.OnPointerDownAsObservable()
            .Subscribe(_ => PlayCatVoice());
        GameObject objectSoundManager = CheckOtherSoundManager();
        soundManager = objectSoundManager.GetComponent<SoundManager>();
    }


    void Update()
    {
        if (gameController.GetSetPlayState < GameController.PlayState.End)
        {
            if (movePosition == (Vector2)gameObjectCpuCat.transform.position)
            {
                movePosition = actionMoveRandomPosition();
            }
            this.gameObjectCpuCat.transform.position = Vector2.MoveTowards(gameObjectCpuCat.transform.position, movePosition, speed * Time.deltaTime);
        } 
    }

    /// <summary>
    /// ランダムな目的地に向かって歩く
    /// </summary>
    /// <returns></returns>
    private Vector2 actionMoveRandomPosition()
    {
        Vector2 randomPosition = new Vector2(Random.Range(-7.5f, 7.5f), gameObjectCpuCat.transform.position.y);
        if (gameObjectCpuCat.transform.position.x <= randomPosition.x)
        {
            gameObjectCpuCat.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObjectCpuCat.GetComponent<SpriteRenderer>().flipX = false;
        }
        return randomPosition;
    }

    /// <summary>
    /// 星との衝突判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            soundManager.PlaySe(catDamageClip);
            collision.gameObject.GetComponent<StarCharacter>().DeleteInstance();
            gameController.DisplayGameOver();
        }
    }

    private GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundManager");
    }

    [ContextMenu("DebugCatVoice")]
    private void PlayCatVoice()
    {
        int num = Random.Range(0, catVoiceClip.Length);
        soundManager.PlaySe(catVoiceClip[num]);
    }
}
