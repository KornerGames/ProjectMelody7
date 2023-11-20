using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _sr;

    private Vector2 _movement;
    private Vector2 _cachedDirection;
    [SerializeField] private bool _isDisabled = false;

    [SerializeField] private AnimationCurve _flashRedAnimCurve;
    [SerializeField] private AnimationCurve _fadeOutAnimCurve;

    public float speed = 1f; // adjust the speed as needed
    public float hurtDuration = 0.1f;
    private PlayerStats _playerStats;

    private void Awake()
    {
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

            if (_movement != Vector2.zero)
            {
                _cachedDirection = _movement;
            }
        }

        // Animation
        _animator.SetFloat("Horizontal", _cachedDirection.x);
        _animator.SetFloat("Vertical", _cachedDirection.y);
        _animator.SetFloat("Speed", _movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        // Movement
        _rb.MovePosition(_rb.position + speed * Time.fixedDeltaTime * _movement);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            OnHurt();
        }
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
        _movement = Vector2.zero;
        StopCoroutine(C_FlashRed());

        //_sr.color = new Color(1f, 1f, 1f, 0f);
        _sr.enabled = false;
    }

    private void OnHurt()
    {
        StartCoroutine(C_FlashRed());
        _playerStats.DeductLife();
    }
}
