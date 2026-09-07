using System;
using TMPro;
using UnityEngine;

public class FeatherTracker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeLeft;

    public void TimeLeft(long UnixTime, int Interval)
    {
        long timeElapsed =  DateTimeOffset.UtcNow.ToUnixTimeSeconds() - UnixTime;
        int timeRemaining = Interval - (int)timeElapsed;
        TimeSpan time = TimeSpan.FromSeconds(timeRemaining);
        if (SaveSystem.instance.currency.playerCurrencyEnergy != 60)
            _timeLeft.text = time.ToString(@"mm\:ss");
        else
            _timeLeft.text = "Plumes Max!";
    }
}
