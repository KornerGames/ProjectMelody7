using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _slashDamage = 10f;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _clipHurt;

    public float SlashDamage => _slashDamage;

    public static event Action NoLives;

    private void Start()
    {
        GameManager.Instance.SetMaxLives(_lives);
    }

    public void DeductLife()
    {
        _lives--;
        GameManager.Instance.UpdateLives(_lives);
        _audioSource.PlayOneShot(_clipHurt);

        if (_lives == 0)
        {
            NoLives?.Invoke();
        }
    }
}
