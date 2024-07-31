using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] private AudioSource finishSound;
    [SerializeField] private GameObject endPanel;

    private void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            finishSound.Play();
            Invoke("CompleteLevel", 1f);
        }
    }

    private void CompleteLevel()
    {
        if (IsLastScene())
        {
            endPanel.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private bool IsLastScene()
    {
        return SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1;
    }
}
