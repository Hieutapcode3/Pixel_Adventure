using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemynotAttack : MonoBehaviour
{
    [SerializeField] protected Transform[] transformsPos;
    [SerializeField] private float stompForce = 10f;
    [SerializeField] private float enemyBounceForce = 4f;
    [SerializeField] private float speed = 2f;
    private bool canMove = true;
    private int currentposition = 0;
    [SerializeField] private float CurrentTime;
    private float RangeTime = 3f;
    private bool isReversing = false;

    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D enemyCollider;
    private bool isHit = false;

    [SerializeField] private Sprite rock1Sprite;
    [SerializeField] private Sprite rock2Sprite;
    [SerializeField] private Sprite rock3Sprite;
    [SerializeField] private RuntimeAnimatorController rock1Anim;
    [SerializeField] private RuntimeAnimatorController rock2Anim;
    [SerializeField] private RuntimeAnimatorController rock3Anim;

    private int rockState = 1;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (canMove)
            Movement();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHit) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (collision.transform.position.y > transform.position.y + 0.1f && playerRb.velocity.y <= 0)
            {
                if (gameObject.name.Contains("Rock"))
                {
                    StartCoroutine(HandleRockHit());
                    playerRb.velocity = new Vector2(-stompForce, stompForce);
                }
                else
                {
                    HandleEnemyHit(playerRb);
                }
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
    private void LowerWaypoints()
    {
        foreach (Transform waypoint in transformsPos)
        {
            Vector3 pos = waypoint.position;
            pos.y -= 0.3f;
            waypoint.position = pos;
        }
    }

    private IEnumerator HandleRockHit()
    {
        switch (rockState)
        {
            case 1:
                anim.SetTrigger("Hit");
                rb.AddForce(new Vector2(0, enemyBounceForce), ForceMode2D.Impulse);
                speed = 0;
                yield return new WaitForSeconds(0.5f);
                GetComponent<SpriteRenderer>().sprite = rock2Sprite;
                anim.runtimeAnimatorController = rock2Anim;
                rockState = 2;
                speed = 2f;
                LowerWaypoints();
                break;
            case 2:
                anim.SetTrigger("Hit");
                rb.AddForce(new Vector2(0, enemyBounceForce), ForceMode2D.Impulse);
                speed = 0;
                yield return new WaitForSeconds(0.5f);
                GetComponent<SpriteRenderer>().sprite = rock3Sprite;
                anim.runtimeAnimatorController = rock3Anim;
                rockState = 3;
                speed = 2f;
                LowerWaypoints();
                break;
            case 3:
                isHit = true;
                anim.SetTrigger("Hit");
                anim.SetBool("Move", false);
                canMove = false;
                StartCoroutine(DestroyAfterDelay());
                break;
        }
    }

    private void HandleEnemyHit(Rigidbody2D playerRb)
    {
        isHit = true;
        anim.SetTrigger("Hit");
        anim.SetBool("Move", false);
        canMove = false;
        if (playerRb != null)
        {
            playerRb.velocity = new Vector2(-stompForce, stompForce);
        }
        StartCoroutine(DestroyAfterDelay());
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
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position == transformsPos[currentposition].position)
        {
            anim.SetBool("Move", false);
            CurrentTime += Time.deltaTime;
            if (CurrentTime >= RangeTime)
            {
                anim.SetBool("Move", true);
                CurrentTime = 0f;

                if (!isReversing)
                {
                    currentposition++;
                    if (currentposition >= transformsPos.Length)
                    {
                        currentposition = transformsPos.Length - 1;
                        isReversing = true;
                    }
                }
                else
                {
                    currentposition--;
                    if (currentposition < 0)
                    {
                        currentposition = 0;
                        isReversing = false;
                    }
                }
            }

            if (currentposition % 2 != 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}

