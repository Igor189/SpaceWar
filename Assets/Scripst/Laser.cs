using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int _damage = 10;
    [SerializeField] private float _timeToActive = 1;
    private SpriteRenderer _spriteRenderer;
    private Color _colorStart;
    private Color _colorEnd;
    private bool _isActive = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _colorEnd = _spriteRenderer.color;
        _colorStart = _colorEnd;
        _colorStart.a = 0;
    }
    private void OnEnable()
    {
        _spriteRenderer.color = _colorStart;
    }
    public IEnumerator Shoot()
    {
        float animationLerp = 0;
        while (animationLerp < 1)
        {
            _spriteRenderer.color = Color.Lerp(_colorStart, _colorEnd, animationLerp);
            animationLerp += Time.deltaTime / _timeToActive;
            yield return null;
        }
        _isActive = true;
        yield return new WaitForSeconds(0.5f);
        _isActive = false;
        while (animationLerp > 0)
        {
            _spriteRenderer.color = Color.Lerp(_colorStart, _colorEnd, animationLerp);
            animationLerp -= Time.deltaTime / _timeToActive;
            yield return null;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
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
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
