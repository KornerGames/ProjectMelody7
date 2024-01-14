using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashArea : MonoBehaviour
{
    private float _damage = 0f;
    private List<Collider2D> _enemies = new List<Collider2D>();
    private static object _lock = new object();

    private Collider2D _ene;

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SlashEnemies()
    {
        if(_ene == null)
        {
            return;
        }

        if (_ene.TryGetComponent(out Enemy e))
        {
            e.TakeDamage(_damage);
        }
        if (_ene.TryGetComponent(out Zac.BaseCharacterStats stats))
        {
            stats.InflictHP(Zac.InflictHPType.Damage, (int)_damage);
        }
        return;

        //lock (_lock)
        //{
        //    foreach (Collider2D enemy in _enemies)
        //    {
        //        if (enemy.TryGetComponent(out Enemy e))
        //        {
        //            e.TakeDamage(_damage);
        //        }
        //        if (enemy.TryGetComponent(out Zac.BaseCharacterStats stats))
        //        {
        //            stats.InflictHP(Zac.InflictHPType.Damage, (int)_damage);
        //        }
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _ene = collision;

        lock (_lock)
        {
            if (collision.CompareTag("Enemy"))
            {
                _enemies.Add(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_ene == collision)
        {
            _ene = null;
        }

        lock (_lock)
        {
            if (collision.CompareTag("Enemy"))
            {
                _enemies.Remove(collision);
            }
        }
    }
}