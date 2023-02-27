using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : BaseEnemyShip
{
    [SerializeField] private ExplosiveCircle _explosiveCircle;
    [SerializeField] private SpriteRenderer _explosiveIndicator;
    [SerializeField] private float _startAlpha = 0.1f;
    [SerializeField] private float _endAlpha = 0.6f;
    [SerializeField] private float _fickerTime = 0.5f;
    private Color _startColor;
    private Color _endColor;
    private StageShip _currentStage;

    private void Awake()
    {
        _startColor = _explosiveIndicator.color;
        _startColor.a = _startAlpha;
        _endColor = _startColor;
        _endColor.a = _endAlpha;
    }

    private IEnumerator LocalUpdate()
    {
        _explosiveCircle._isActive = true;
        while (_currentStage == StageShip.Wait)
        {
            if (_explosiveCircle._isActive)
            {
                float animationLerp = 0;
                while (animationLerp < 1)
                {
                    _explosiveIndicator.color = Color.Lerp(_startColor, _endColor, animationLerp);
                    animationLerp += Time.deltaTime / _fickerTime;
                    yield return null;
                }
                while (animationLerp > 0)
                {
                    _explosiveIndicator.color = Color.Lerp(_startColor, _endColor, animationLerp);
                    animationLerp -= Time.deltaTime / _fickerTime;
                    yield return null;
                }
            }
            else
            {
                _explosiveIndicator.color = _explosiveCircle._clearColor;
            }
        }
        _explosiveCircle._isActive = false;
        _explosiveIndicator.color = _explosiveCircle._clearColor;
    }
    protected override void UpdateStage(StageShip stage)
    {
        _currentStage = stage;
        if (stage == StageShip.Wait)
        {
            StartCoroutine(LocalUpdate());
        }
    }
}
