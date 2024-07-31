using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class FanControll : MonoBehaviour
{
    [SerializeField] private float              downForce = 2f;
    private bool                                checkCollision = false;
    private Vector3                             defaultPos;
    [SerializeField] private GameObject         fanEffect;

    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        defaultPos = transform.position;
        fanEffect.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(!checkCollision)
                StartCoroutine(CollisionEffect());
            StartCoroutine(TurnOff());
        }
    }

    private IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("Off");
        fanEffect.SetActive(false);
        yield return new WaitForSeconds(1f);
        col.enabled = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    private IEnumerator CollisionEffect()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(new Vector2(0, -downForce), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        rb.bodyType = RigidbodyType2D.Static;
        checkCollision = true;
        while (Vector3.Distance(transform.position, defaultPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, defaultPos, downForce * Time.deltaTime);
            yield return null; 
        }
        transform.position = defaultPos;
    }
}
