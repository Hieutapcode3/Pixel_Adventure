using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float trapForce = 20f;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player")){
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = (Vector2.up * trapForce);
            anim.SetTrigger("Push");
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.DisableDoubleJump();
            }
        }
    }
}
