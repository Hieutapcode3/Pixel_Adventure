using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private AudioSource    deathSoundEffect;
    public static PlayerLife                Instance;

    private Animator                        anim;
    private Rigidbody2D                     rb;
    private BoxCollider2D                   col;

    [SerializeField] private PlayerMovement player;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            deathSoundEffect.Play();
            Die();
        }
    }
    public void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        col.enabled = false;
        anim.SetTrigger("Death");
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        player.SetSkin();
    }
}
