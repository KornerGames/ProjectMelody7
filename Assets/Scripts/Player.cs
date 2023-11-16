using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private RaycastHit2D hit;

    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _sr;

    private Vector2 _movement;
    private bool _isDisabled = false;

    public float speed = 1f; // adjust the speed as needed
    public float hurtDuration = 0.1f;
    private PlayerStats _playerStats;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();

        _playerStats = GetComponent<PlayerStats>();
    }

    private void OnEnable()
    {
        PlayerStats.NoLives += OnDie;
    }

    private void OnDisable()
    {
        PlayerStats.NoLives -= OnDie;
    }

    private void Update()
    {
        if (!_isDisabled)
        {
            // Input
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
        }

        // Animation
        _animator.SetFloat("Horizontal", _movement.x);
        _animator.SetFloat("Vertical", _movement.y);
        _animator.SetFloat("Speed", _movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        // Movement
        _rb.MovePosition(_rb.position + speed * Time.fixedDeltaTime * _movement);
    }

    private IEnumerator C_FlashRed()
    {
        _sr.color = Color.red;
        yield return new WaitForSeconds(hurtDuration);
        _sr.color = Color.white;
    }

    private void OnDie()
    {
        _isDisabled = true;
        _sr.color = new Color(1, 1, 1, 0);
    }

    private void OnHurt()
    {
        _playerStats.DeductLife();
        StartCoroutine(C_FlashRed());
    }
}
