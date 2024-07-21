using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float doubleJumpForce = 12f;
    [SerializeField] private float wallSlideSpeed = 2f;
    private bool isTouchingFront;
    private bool wallSlide;


    public Transform StartPoint;
    private bool canDoubleJump;

    public PlayerSelect skinPlayer;

    [SerializeField] private GameObject dustLeft;
    [SerializeField] private GameObject dustRight;
    [SerializeField] private GameObject jumpEffect;

    private enum MovementState { idle, running, jumping, falling, wallSlide, doubleJump }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        SetSkin();
        transform.position = new Vector2(StartPoint.position.x + 0.6f, StartPoint.position.y);
    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown("space"))
        {
            jumpSoundEffect.Play();
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = true;
                StartCoroutine(ActivateJumpEffect());
            }
            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                canDoubleJump = false;
                StartCoroutine(ActivateJumpEffect());
            }
        }
        if(isTouchingFront && !IsGrounded())
        {
            wallSlide = true;
        }
        else
        {
            wallSlide = false;

        }
        if (wallSlide)
        {
            anim.SetBool("WallSlide", true);
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle;

        if (dirX > 0 && !wallSlide)
        {
            state = MovementState.running;
            sprite.flipX = false;
            dustRight.SetActive(false);
            dustLeft.SetActive(true);
        }
        else if (dirX < 0 && !wallSlide)
        {
            state = MovementState.running;
            sprite.flipX = true;
            dustRight.SetActive(true);
            dustLeft.SetActive(false);
        }
        else if(dirX ==0 && !wallSlide)
        {
            state = MovementState.idle;
            dustRight.SetActive(false);
            dustLeft.SetActive(false);
        }
        else
        {
            dustRight.SetActive(false);
            dustLeft.SetActive(false);
        }
            

        if (rb.velocity.y > 0.1f)
        {
            if (canDoubleJump)
                state = MovementState.jumping;
            else
                state = MovementState.doubleJump;
        }
        else if (rb.velocity.y < -0.1f && !wallSlide)
        {
            state = MovementState.falling;
            anim.SetBool("WallSlide", false);
        }

        anim.SetInteger("State", (int)state);
        
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private IEnumerator ActivateJumpEffect()
    {
        jumpEffect.transform.position = transform.position;
        jumpEffect.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        jumpEffect.SetActive(false);
    }

    public void DisableDoubleJump()
    {
        canDoubleJump = true;
    }

    public void SetSkin()
    {
        if (SkinManager.Instance != null)
        {
            SkinManager.Instance.SetMainPlayer(skinPlayer);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("WallSlide"))
        {
            isTouchingFront = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isTouchingFront = false;
    }
}
