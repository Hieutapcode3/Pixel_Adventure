using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
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
