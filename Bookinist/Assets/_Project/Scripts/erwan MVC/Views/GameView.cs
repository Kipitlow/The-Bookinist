using TMPro;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _playerOnePowerText;
    [SerializeField] private TextMeshProUGUI _playerTwoPowerText;

    private int _totalPower;

    public void Init(int totalPower, int playerOnePower, int playerTwoPower)
    {
        _totalPower = totalPower;
        _playerOnePowerText.text = playerOnePower.ToString() + " / " + _totalPower;
        _playerTwoPowerText.text = playerTwoPower.ToString() + " / " + _totalPower;
    }
    public void UpdateTimer(float remainTime)
    {
        _timerText.text = "Time : " + Mathf.CeilToInt(remainTime).ToString();
    }

    public void UpdatePlayerPower(bool isPlayerOne, int power)
    {
        if (isPlayerOne)
            _playerOnePowerText.text = power.ToString() + " / " + _totalPower;
        else
            _playerTwoPowerText.text = power.ToString() + " / " + _totalPower;
    }
}
