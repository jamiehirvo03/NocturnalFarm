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

    public event Action onPlayerInDarkness;
    public event Action onPlayerInSunlight;

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
    public void PlayerInDarkness()
    {
        if (onPlayerInDarkness != null)
        {
            onPlayerInDarkness();
        }
    }
    public void PlayerInSunlight()
    {
        if (onPlayerInSunlight != null)
        {
            onPlayerInSunlight();
        }
    }
}
