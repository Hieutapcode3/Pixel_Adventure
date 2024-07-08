using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
    public enum Player { Guy, PinkMan, Frog, Mask };
    public Player playerSelected;

    public Animator anim;
    public SpriteRenderer spriteRenderer;

    public RuntimeAnimatorController[] playerController;
    public Sprite[] playerRender;

    void Start()
    {
        UpdatePlayer();
    }

    public void NextSkin()
    {
        playerSelected = (Player)(((int)playerSelected + 1) % System.Enum.GetValues(typeof(Player)).Length);
        UpdatePlayer();
    }

    public void PreviousSkin()
    {
        playerSelected = (Player)(((int)playerSelected - 1 + System.Enum.GetValues(typeof(Player)).Length) % System.Enum.GetValues(typeof(Player)).Length);
        UpdatePlayer();
    }

    public void SetSkin(Player selectedPlayer)
    {
        playerSelected = selectedPlayer;
        UpdatePlayer();
    }

    void UpdatePlayer()
    {
        switch (playerSelected)
        {
            case Player.Guy:
                spriteRenderer.sprite = playerRender[0];
                anim.runtimeAnimatorController = playerController[0];
                break;
            case Player.PinkMan:
                spriteRenderer.sprite = playerRender[1];
                anim.runtimeAnimatorController = playerController[1];
                break;
            case Player.Mask:
                spriteRenderer.sprite = playerRender[3];
                anim.runtimeAnimatorController = playerController[3];
                break;
            case Player.Frog:
                spriteRenderer.sprite = playerRender[2];
                anim.runtimeAnimatorController = playerController[2];
                break;
        }
    }
}
