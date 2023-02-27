using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCircle : MonoBehaviour
{
    public bool _isActive = false;
    [SerializeField] private int _damage = 20;
    private SpriteRenderer _spriteRenderer;
    public Color _clearColor;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _clearColor = _spriteRenderer.color;
        _clearColor.a = 0;
    }

    private void OnEnable()
    {
        _spriteRenderer.color = _clearColor;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isActive)
        {
            var obj = collision.gameObject;
            if (obj.TryGetComponent(out PlayerShip player))
            {
                player.DamageMe(_damage);
                _isActive = false;
            }
        }
    }
}
