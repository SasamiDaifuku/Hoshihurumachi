using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField]
    float groundCheckRadius = 0.4f;
    [SerializeField]
    float groundCheckOffsetY = 0.45f;
    [SerializeField]
    float groundCheckDistance = 0.2f;

    private Rigidbody2D rigidbody2DPlayer;
    private bool isGround;

    [SerializeField] private GameController gameController;

    private Animator animator;

    void Start()
    {
        rigidbody2DPlayer = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (gameController.GetSetPlayState != GameController.PlayState.End)
        {
            //設置判定
            isGround = CheckGroundStatus();
            if (Input.GetKeyDown("space") && isGround)
            {
                //着地判定をfalse
                isGround = false;
                //AddForceにて上方向へ力を加える
                rigidbody2DPlayer.AddForce(Vector2.up * jumpPower);
            }
            float horizontalKey = Input.GetAxisRaw("Horizontal");
            float xSpeed = 0.0f;
            if (horizontalKey > 0)
            {
                transform.localScale = new Vector2(-1, 1);
                xSpeed = speed;
                animator.SetBool("Run",true);
            }
            else if (horizontalKey < 0)
            {
                transform.localScale = new Vector2(1, 1);
                xSpeed = -speed;
                animator.SetBool("Run", true);
            }
            else
            {
                xSpeed = 0.0f;
                animator.SetBool("Run", false);
            }
            rigidbody2DPlayer.velocity = new Vector2(xSpeed, rigidbody2DPlayer.velocity.y);
        }else
        {
            animator.SetBool("Run", false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<StarCharacter>().DeleteInstance();
        }
    }
    private RaycastHit2D CheckGroundStatus()
    {
        RaycastHit2D hit = Physics2D.CircleCast((Vector2)transform.position + groundCheckOffsetY * Vector2.up, groundCheckRadius, Vector2.down, groundCheckDistance, groundLayer);
        return hit;
    }
}
