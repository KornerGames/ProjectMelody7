using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _hp = 30f;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        _hp -= damage;
        if (_hp > 0)
        {
            _animator.SetTrigger("Hurt");
        }
        else
        {
            _animator.SetTrigger("Die");
        }
    }

    public void OnEndDieAnimation()
    {
        Destroy(gameObject);
    }
}
