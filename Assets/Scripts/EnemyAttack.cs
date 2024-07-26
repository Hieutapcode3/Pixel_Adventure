using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Transform[]            points;  
    [SerializeField] private float                  speed = 2f;  
    [SerializeField] private float                  raycastDistance = 9f; 
    [SerializeField] private LayerMask              playerLayer;  
    [SerializeField] private PosSpawnBullet         posSpawnBullet;  

    private bool                                    movingToB = true;
    private bool                                    isMoving = true;

    private Animator                                anim;

    void Start()
    {
        if (posSpawnBullet == null)
        {
            posSpawnBullet = FindObjectOfType<PosSpawnBullet>();
        }
        StartCoroutine(MoveBetweenPoints());
        anim = GetComponent<Animator>();
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, playerLayer);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (isMoving)
            {
                anim.SetBool("Attack", true);
                isMoving = false;  
                StopAllCoroutines();  
            }
        }
        else
        {
            if (!isMoving)
            {
                anim.SetBool("Attack", false);
                isMoving = true; 
                StartCoroutine(MoveBetweenPoints());  
            }
        }

        Debug.DrawRay(transform.position, Vector2.down * raycastDistance, Color.red);
    }
    private void EnemyShoot()
    {
        if (posSpawnBullet != null)
        {
            posSpawnBullet.Shoot();
        }
    }
}
