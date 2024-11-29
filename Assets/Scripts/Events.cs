using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events current;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
    }

    public event Action onPlayerIdle;

    public event Action onPlayerMoveUp;
    public event Action onPlayerMoveDown;
    public event Action onPlayerMoveLeft;
    public event Action onPlayerMoveRight;

    public event Action onPlayerLightAttack;
    public event Action onPlayerHeavyAttack;
    public event Action onPlayerRangedAttack;

    public event Action onShowEnemyUI;
    public event Action onHideEnemyUI;

    public void PlayerIdle()
    {
        if (onPlayerIdle != null)
        {
            onPlayerIdle();
        }
    }
    public void PlayerMoveUp()
    {
        if (onPlayerMoveUp != null)
        {
            onPlayerMoveUp();
        }
    }
    public void PlayerMoveDown()
    {
        if (onPlayerMoveDown != null)
        {
            onPlayerMoveDown();
        }
    }
    public void PlayerMoveLeft()
    {
        if (onPlayerMoveLeft != null)
        {
            onPlayerMoveLeft();
        }
    }
    public void PlayerMoveRight()
    {
        if (onPlayerMoveRight != null)
        {
            onPlayerMoveRight();
        }
    }
    public void PlayerLightAttack()
    {
        if (onPlayerLightAttack != null)
        {
            onPlayerLightAttack();
        }
    }
    public void PlayerHeavyAttack()
    {
        if (onPlayerHeavyAttack != null)
        {
            onPlayerHeavyAttack();
        }
    }
    public void PlayerRangedAttack()
    {
        if (onPlayerRangedAttack != null)
        {
            onPlayerRangedAttack();
        }
    }
    public void ShowEnemyUI()
    {
        if (onShowEnemyUI != null)
        {
            onShowEnemyUI();
        }
    }
    public void HideEnemyUI()
    {
        if (onHideEnemyUI != null)
        {
            onHideEnemyUI();
        }
    }
}
