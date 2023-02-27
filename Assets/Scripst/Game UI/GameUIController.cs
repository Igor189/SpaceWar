using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private Text _countHelth;
    [SerializeField] private Text _countScore;
    [SerializeField] private Text _countMoney;
    [SerializeField] private Text _maxScore;
    [SerializeField] private Slider _barHealth;
    [SerializeField] private Text _countScoreWindowGameOver;
    [SerializeField] private GameObject _windowGameOver;

    private CompositeDisposable _disposables = new CompositeDisposable();
    private Canvas _canvas;

    private void Start()
    {
        var controller = Controller.Instance;

        controller.OnGameOver.Subscribe((_)=>ShowWindowGameOver()).AddTo(_disposables);
        controller._myShip._health.Subscribe(UpdateBar).AddTo(_disposables);
        controller.Score.Subscribe(UpdateScore).AddTo(_disposables);
        controller._myShip._money.Subscribe(UpdateMoney).AddTo(_disposables);
        _canvas = GetComponent<Canvas>();
    }
    private void UpdateBar(int value)
    {
        _barHealth.value = (float)value/100;
        _countHelth.text = value.ToString();
    }
    private void UpdateScore(int score)
    {
        if (!_windowGameOver.activeSelf)
        {
            _countScore.text = score.ToString();
        }
    }
    private void UpdateMoney(int money)
    {
        if (!_windowGameOver.activeSelf)
        {
            _countMoney.text = money.ToString();
        }
    }
    public void ShowWindowGameOver()
    {
        _countScoreWindowGameOver.text = "Score: "+Controller.Instance.Score.Value.ToString();
        _maxScore.text = "Max score: " + Controller.Instance._maxScore.ToString();
        _windowGameOver.SetActive(true);
        _canvas.sortingOrder = 1;
    }
    public void ClickToMainMenu()
    {
        LevelManager.PlayScene(Scenes.MainMenu);
        gameObject.SetActive(false);
        _canvas.sortingOrder = 0;
    }
    public void ClickRestart()
    {
        LevelManager.PlayScene(Scenes.Game);
        gameObject.SetActive(false);
        _canvas.sortingOrder = 0;
    }
    private void OnDestroy()
    {
        if (_disposables != null)
        {
            _disposables.Dispose();
            _disposables = null;
        }
    }
}
