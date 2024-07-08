using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public PlayerSelect playerIdle;
    public PlayerSelect playerRun;

    public void NextSkin()
    {
        playerIdle.NextSkin();
        playerRun.SetSkin(playerIdle.playerSelected);
    }

    public void PreviousSkin()
    {
        playerIdle.PreviousSkin();
        playerRun.SetSkin(playerIdle.playerSelected);
    }
}
