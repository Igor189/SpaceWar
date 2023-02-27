using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeWindow : MonoBehaviour
{
    [SerializeField] private Text _damageText;
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _moneyText;
    [SerializeField] private Text _costHealthText;
    [SerializeField] private Text _costDamageText;
    private Controller _controller;
    private Dictionary<int, int> _healthCost = new Dictionary<int, int>()
    {
        {100,30},
        {105,70 },
        {110,120},
        {115,180 }
    };
    private Dictionary<int, int> _damageCost = new Dictionary<int, int>()
    {
        {100,30},
        {101,70 },
        {102,120 },
        {103,180 }
    };

    private void Awake()
    {
        UpdateText();
    }

    public void ClickUpdateHealth()
    {
        if(_healthCost.ContainsKey(Controller.Instance._health)&&Controller.Instance._currentMoney>=_healthCost[Controller.Instance._health])
        {
            Controller.Instance._currentMoney -= _healthCost[Controller.Instance._health];
            Controller.Instance._health += 5;
            Controller.Instance.SaveGame();
            UpdateText();
        }
    }

    public void ClickUpdateDamage()
    {
        if (_damageCost.ContainsKey(Controller.Instance._damage) && Controller.Instance._currentMoney >= _damageCost[Controller.Instance._damage])
        {
            Controller.Instance._currentMoney -= _damageCost[Controller.Instance._damage];
            Controller.Instance._damage++;
            Controller.Instance.SaveGame();
            UpdateText();
        }
    }

    private void UpdateText()
    {
        _controller = Controller.Instance;
        _damageText.text = _controller._damage.ToString();
        _healthText.text = _controller._health.ToString();
        _moneyText.text = _controller._currentMoney.ToString();
        if(_healthCost.ContainsKey(_controller._health))
        {
            _costHealthText.text ="Улучшить:"+ _healthCost[_controller._health].ToString();
        }
        else
        {
            _costHealthText.text = "MAX";
        }
        if(_damageCost.ContainsKey(_controller._damage))
        {
            _costDamageText.text = "Улучшить:" + _damageCost[_controller._damage].ToString();
        }
        else
        {
            _costDamageText.text = "MAX";
        }
    }
}
