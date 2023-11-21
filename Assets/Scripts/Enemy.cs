using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _hp = 30f;

    public void TakeDamage(float damage)
    {
        _hp -= damage;
        Debug.Log($"Enemy took damage. HP is now {_hp}");
        if (_hp <= 0)
        {
            Debug.Log("Enemy died");
        }
    }
}
