using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private LayerMask jumpalbleGround;


    private float dirX =0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float doubleJumpForce = 12f;
    public Transform startPoint;
    private bool canDoubleJump;

    public PlayerSelect skinPlayer;

    private enum MovementState {idel,running,jumping,falling,wallSlide,doubleJump}
 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        SetSkin();
        transform.position = new Vector2(startPoint.position.x + 0.6f,startPoint.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2 (dirX * moveSpeed, rb.velocity.y);
        if (Input.GetKeyDown("space"))
        {
            jumpSoundEffect.Play();
            if (isGround())
            {
                rb.velocity = new Vector3(rb.velocity.x,jumpForce,0);
                canDoubleJump = true;
            }
            else
            {
                if(Input.GetKeyDown("space") && canDoubleJump)
                {
                    rb.velocity = new Vector3(rb.velocity.x, doubleJumpForce, 0);
                    canDoubleJump =false;
                }
            }
        }
        UpdateAnimationState();
    }
    private void UpdateAnimationState()
    {
        MovementState State;
        if (dirX > 0)
        {
            State = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0)
        {
            State = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            State = MovementState.idel;
            
        }
        if (rb.velocity.y > .1f)
        {
            if(canDoubleJump)
                State = MovementState.jumping;
            else
                State = MovementState.doubleJump;
        }
        else if(rb.velocity.y < -.1f)
        {
            State = MovementState.falling;
        }
        anim.SetInteger("State", (int)State);
    }
    private bool isGround()
    {
        return Physics2D.BoxCast(coll.bounds.center,coll.bounds.size,0f,Vector2.down,.1f,jumpalbleGround);
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
}
