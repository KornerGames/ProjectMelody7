using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ASingleton<GameManager>
{
    [SerializeField] private GameUIView _view;

    public void UpdateLives(int value)
    {
        _view.UpdateLives(value);
    }
}
