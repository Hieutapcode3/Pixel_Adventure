using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyhit : MonoBehaviour
{
    [SerializeField] protected Transform[] transformsPos;
    [SerializeField] private float stompForce = 5f; 
    [SerializeField] private float enemyBounceForce = 4f;
    [SerializeField] private float Speed = 2f;
    private bool canMove = true;
    private int currentposition = 0;
    [SerializeField] private float CurrentTime;
    private float RangeTime = 3f;
    



    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D enemyCollider;
    private bool isHit = false; 

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if(canMove) 
            Movement();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHit) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Debug.Log(playerRb.velocity.y);
            if (collision.transform.position.y > transform.position.y + 0.1f && playerRb.velocity.y <= 0)
            {
                isHit = true;
                anim.SetTrigger("Hit");
                anim.SetBool("Move", false);
                canMove = false;
                if (playerRb != null)
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, stompForce);
                }

                StartCoroutine(DestroyAfterDelay());
            }
            else
            {
                PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();
                if (playerLife != null)
                {
                    playerLife.Die();
                }
            }
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        enemyCollider.enabled = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(new Vector2(0, enemyBounceForce), ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    protected void Movement()
    {
        anim.SetBool("Move", true);
        Vector3 target = transformsPos[currentposition].position;
        transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);
        if (transform.position == transformsPos[currentposition].position)
        {
            anim.SetBool("Move", false);
            CurrentTime += Time.deltaTime;
            if (CurrentTime >= RangeTime)
            {
                anim.SetBool("Move", true);
                CurrentTime = 0f;
                currentposition++;
                if (currentposition >= transformsPos.Length)
                {
                    currentposition = 0;
                }

            }
            if (currentposition == 1)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            else
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        }
    }
}
