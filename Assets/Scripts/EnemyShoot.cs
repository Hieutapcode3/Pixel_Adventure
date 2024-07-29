using System.Collections;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float raycastDistance = 9f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private PosSpawnBullet posSpawnBullet;
    [SerializeField] private Vector2 raycastDirection = Vector2.down;
    [SerializeField] private float stompForce = 10f;
    [SerializeField] private float enemyBounceForce = 4f;

    private bool movingToB = true;
    private bool isMoving = true;

    private Animator anim;
    private BoxCollider2D col;
    private Rigidbody2D rb;

    void Start()
    {
        if (posSpawnBullet == null)
        {
            posSpawnBullet = FindObjectOfType<PosSpawnBullet>();
        }
        if (points.Length != 0)
        {
            StartCoroutine(MoveBetweenPoints());
        }
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        CheckForPlayer();
    }

    private IEnumerator MoveBetweenPoints()
    {
        while (true)
        {
            if (movingToB)
            {
                transform.position = Vector3.MoveTowards(transform.position, points[1].position, speed * Time.deltaTime);
                if (transform.position == points[1].position)
                {
                    movingToB = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, points[0].position, speed * Time.deltaTime);
                if (transform.position == points[0].position)
                {
                    movingToB = true;
                }
            }
            yield return null;
        }
    }

    private void CheckForPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, raycastDistance, playerLayer);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (isMoving || points.Length == 0)
            {
                anim.SetBool("Attack", true);
                isMoving = false;
                StopAllCoroutines();
            }
        }
        else
        {
            anim.SetBool("Attack", false);
            if (!isMoving && points.Length != 0)
            {
                isMoving = true;
                StartCoroutine(MoveBetweenPoints());
            }
        }

        Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);
    }

    private void EnemyCanShoot()
    {
        if (posSpawnBullet != null)
        {
            posSpawnBullet.Shoot(raycastDirection); 
        }
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
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
