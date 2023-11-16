using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIView : MonoBehaviour
{
    [SerializeField] private Slider _livesSlider;

    public void UpdateLives(int value)
    {
        _livesSlider.value = value;
    }
}
