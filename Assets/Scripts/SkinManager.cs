using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    public PlayerSelect playerIdle;
    public PlayerSelect playerRun;
    public PlayerSelect mainPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Start Screen")
        {
            playerIdle = GameObject.Find("PlayerIdle").GetComponent<PlayerSelect>();
            playerRun = GameObject.Find("PlayerRun").GetComponent<PlayerSelect>();
        }
    }

    public void NextSkin()
    {
        playerIdle.NextSkin();
        playerRun.SetSkin(playerIdle.playerSelected);
        if (mainPlayer != null)
        {
            mainPlayer.SetSkin(playerIdle.playerSelected);
        }
    }

    public void PreviousSkin()
    {
        playerIdle.PreviousSkin();
        playerRun.SetSkin(playerIdle.playerSelected);
        if (mainPlayer != null)
        {
            mainPlayer.SetSkin(playerIdle.playerSelected);
        }
    }

    public void SetMainPlayer(PlayerSelect newMainPlayer)
    {
        mainPlayer = newMainPlayer;
        mainPlayer.SetSkinBySave();
    }
}
