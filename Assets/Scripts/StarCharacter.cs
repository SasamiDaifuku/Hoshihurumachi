using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCharacter : MonoBehaviour
{
    private Transform canvasTran;
    private float time = 0f;
    [SerializeField] private float interval = 1.5f;
    private SoundManager soundManager;
    [SerializeField] private AudioClip destroyClip;
    [SerializeField] private GameObject explosionAnimation;
    [SerializeField] private Color[] starColor;
    /// <summary>
    /// 星の設定
    /// </summary>
    /// <param name="canvasTran"></param>
    public void SetUpStarCharacter(Transform canvasTran)
    {
        this.canvasTran = canvasTran;
        GameObject objectSoundManager = CheckOtherSoundManager();
        soundManager = objectSoundManager.GetComponent<SoundManager>();
        DecideColor();
        
    }

    /// <summary>
    /// 色をランダムで決定する
    /// </summary>
    private void DecideColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        int num = Random.Range(0, starColor.Length);
        Debug.Log("星の色" + spriteRenderer.color);
        Debug.Log("指定" + starColor[num]);
        spriteRenderer.color = starColor[num];
        Debug.Log("星の色 変化後" + spriteRenderer.color);
    }

    private GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundManager");
    }

    void FixedUpdate()
    {
        //時間計測
        time += Time.deltaTime;
        if (time > interval)
        {
            FallStar();
        }
    }
    /// <summary>
    /// 星を落とす
    /// </summary>
    private void FallStar()
    {
        Rigidbody2D rigidbody2DStar = GetComponent<Rigidbody2D>();
        //rigidbody2DStar.velocity = new Vector2(0, ySpeed);
        var acceleration = new Vector2(0,-0.001f);
        rigidbody2DStar.AddForce(acceleration);
    }

    /// <summary>
    /// オブジェクトを消す
    /// </summary>
    public void DeleteInstance()
    {
        //オブジェクト破壊時のSEを鳴らす
        soundManager.PlaySe(destroyClip);
        //爆発アニメーションを表示する
        Instantiate(explosionAnimation, transform.position, transform.rotation);
        //画面外に出たらオブジェクトを消す
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //地面に衝突したら消す
        if(collision.gameObject.tag == "Platform")
        {
            DeleteInstance();
        }
    }
}
