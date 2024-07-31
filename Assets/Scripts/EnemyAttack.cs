using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] protected Transform[]      transformsPos;
    [SerializeField] private float              speed = 1.5f;
    [SerializeField] private float              currentTime;
    [SerializeField] private float              rangeTime = 3f;
    private int                                 currentposition = 0;

    [SerializeField] private float              attackCooldown;
    [SerializeField] private float              range;
    [SerializeField] private float              colliderDistance;
    private float                               cooldownTime = Mathf.Infinity;
    [SerializeField] private LayerMask          playerLayer;
    [SerializeField] private PlayerMovement     player;
    private bool                                isReversing;
    private bool                                isWaiting = false;
    private bool                                playerInRange = false;
    [SerializeField] private float              stompForce = 10f;
    [SerializeField] private float              enemyBounceForce = 4f;

    private Animator                            anim;
    [SerializeField] private BoxCollider2D      col;
    private Rigidbody2D                         rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        cooldownTime += Time.deltaTime;

        if (PlayerInSight())
        {
            playerInRange = true;

            Vector3 direction = player.transform.position - transform.position;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            if (cooldownTime >= attackCooldown)
            {
                anim.SetTrigger("Attack");
                anim.SetBool("Move", false);
                cooldownTime = 0;
            }

            UpdateTargetPositionBasedOnPlayer();
        }
        else
        {
            if (playerInRange)
            {
                playerInRange = false;
                StartCoroutine(WaitBeforeMoving());
            }
            else if (!isWaiting)
            {
                Movement();
            }
        }
    }

    private void UpdateTargetPositionBasedOnPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        if (direction.x > 0)
        {
            if (!isReversing && currentposition < transformsPos.Length - 1)
            {
                currentposition++;
            }
            else if (isReversing && currentposition > 0)
            {
                currentposition--;
            }
            else
            {
                isReversing = !isReversing;
            }
        }
        else
        {
            if (isReversing && currentposition < transformsPos.Length - 1)
            {
                currentposition++;
            }
            else if (!isReversing && currentposition > 0)
            {
                currentposition--;
            }
            else
            {
                
                isReversing = !isReversing;
            }
        }
    }

    private IEnumerator WaitBeforeMoving()
    {
        isWaiting = true;
        yield return new WaitForSeconds(1f); 
        isWaiting = false;
        Movement();
    }

    protected void Movement()
    {
        anim.SetBool("Move", true);
        Vector3 target = transformsPos[currentposition].position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position == transformsPos[currentposition].position)
        {
            anim.SetBool("Move", false);
            currentTime += Time.deltaTime;
            if (currentTime >= rangeTime)
            {
                anim.SetBool("Move", true);
                currentTime = 0f;

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

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(col.bounds.size.x * range, col.bounds.size.y, col.bounds.size.z),
            0, Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }
    private void PlayerTakeDamage()
    {
        if (PlayerInSight())
            PlayerLife.Instance.Die();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(col.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(col.bounds.size.x * range, col.bounds.size.y, col.bounds.size.z));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (collision.transform.position.y > transform.position.y + 0.1f && playerRb.velocity.y <= 0)
            {
                HandleEnemyHit(playerRb);
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

    private void HandleEnemyHit(Rigidbody2D playerRb)
    {
        Debug.Log("HandleEnemyHit called");
        anim.SetTrigger("Hit");
        if (playerRb != null)
        {
            playerRb.velocity = new Vector2(-stompForce, stompForce);
            Debug.Log("Player velocity set");
        }
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        col.enabled = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(new Vector2(0, enemyBounceForce), ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
