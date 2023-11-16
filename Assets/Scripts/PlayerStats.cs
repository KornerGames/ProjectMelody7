using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int lives = 3;

    public static event Action NoLives;
    public static event Action DeductedLives;

    public void DeductLife()
    {
        lives--;
        DeductedLives?.Invoke();

        if (lives == 0)
        {
            NoLives?.Invoke();
        }
    }
}
