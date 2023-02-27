using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Controller : MonoBehaviour
{
    public HealthBonus _healthBonusPref;
    public int _procentBonusHealth = 30;
    public MoneyBonus _moneyBonusPref;
    public int _procentBonusMoney = 10;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _effectSource;
    [SerializeField] private AudioClip _clipShot;

    public int _health = 100;
    public int _damage = 100;
    public int _maxScore = 0;
    public int _currentMoney = 0;

    public ReactiveProperty<int> Score = new ReactiveProperty<int>();

    public AudioSource MusicSource => _musicSource;
    public AudioSource EffectSource => _effectSource;

    public static Controller Instance;
    public PlayerShip _myShip;

    private Vector3 _leftDownPoint;
    private Vector3 _rightDownPoint;
    private Vector3 _leftUpPoint;
    private Vector3 _rightUpPoint;
    private Vector2 _centrCam;

    public Vector3 LeftDownPoint => _leftDownPoint;
    public Vector3 RightDownPoint => _rightDownPoint;
    public Vector3 LeftUpPoint => _leftUpPoint;
    public Vector3 RightUpPoint => _rightUpPoint;
    public Vector2 CentrCam => _centrCam;

    private Subject<Unit> _gameOver = new Subject<Unit>();
    public IObservable<Unit> OnGameOver => _gameOver;

    private void Awake()
    {
        Instance = this;
        Instance.LoadGame();
    }
    public void UpdateCameraSettings()
    {
        var cameraMain = Camera.main;
        if (cameraMain != null)
        {
            _centrCam = cameraMain.transform.position;

            float distance = cameraMain.farClipPlane;
            _leftDownPoint = cameraMain.ScreenToWorldPoint(new Vector3(0, 0, distance));
            _rightDownPoint = cameraMain.ScreenToWorldPoint(new Vector3(cameraMain.pixelWidth, 0, distance));
            _leftUpPoint = cameraMain.ScreenToWorldPoint(new Vector3(0, cameraMain.pixelHeight, distance));
            _rightUpPoint = cameraMain.ScreenToWorldPoint(new Vector3(cameraMain.pixelWidth, cameraMain.pixelHeight, distance));
        }
    }
    public void GameOver()
    {
        if(Score.Value>_maxScore)
        {
            _maxScore = Score.Value;
        }
        _currentMoney += _myShip._money.Value;
        SaveGame();
        _gameOver.OnNext(Unit.Default);
    }
    public void PlayAudioShot()
    {
        _effectSource.PlayOneShot(_clipShot);
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + "/MySaveData.dat");
        SaveData data = new SaveData();
        data._savedHealth = _health;
        data._savedDamage = _damage;
        data._savedScore = _maxScore;
        data._savedMoney = _currentMoney;
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/MySaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            _health = data._savedHealth;
            _damage = data._savedDamage;
            _maxScore = data._savedScore;
            _currentMoney = data._savedMoney;
        }
    }
}

[Serializable]
class SaveData
{
    public int _savedHealth;
    public int _savedDamage;
    public int _savedScore;
    public int _savedMoney;
}
