using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class KamiHunter : BaseEnemyShip
{
    [SerializeField] private float _coolDown = 3f;
    private float _coolDownCurrent;
    private StageShip _currentStage;
    [SerializeField] private Laser _laser;

    private IEnumerator LookAtPlayer()
    {
        if (_player == null)
        {
            yield break;
        }
        while (_currentStage == StageShip.Wait)
        {
            if (_coolDownCurrent < _coolDown)
            {
                _coolDownCurrent += Time.deltaTime;
            }
            else
            {
                StartCoroutine(_laser.Shoot());
                _coolDownCurrent = 0;
            }
            Look(_player.transform.position - transform.position, true, true);
            yield return null;
        }
    }
    
    protected override void UpdateStage(StageShip stage)
    {
        _currentStage = stage;

        switch (_currentStage)
        {
            case StageShip.In:
                break;
            case StageShip.Wait:
                StartCoroutine(LookAtPlayer());
                break;
            case StageShip.Out:
                StopCoroutine(_laser.Shoot());
                _playerLastPos = _player.transform.position;
                break;
        }
    }
}
