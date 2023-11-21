using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashArea : MonoBehaviour
{
    private float _damage = 0f;
    private List<Collider2D> _enemies = new List<Collider2D>();

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SlashEnemies()
    {
        foreach(Collider2D enemy in _enemies)
        {
            if (enemy.TryGetComponent(out Enemy e))
            {
                e.TakeDamage(_damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _enemies.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _enemies.Remove(collision);
        }
    }
}