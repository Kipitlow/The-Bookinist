using System;
using TMPro;
using UnityEngine;
using System.Collections;

public class EnergyIncrementer : MonoBehaviour
{
    private const int _regenInterval = 10; //Le temps pour regen 1 energie (En secondes) 1440 = 24 minutes
    [SerializeField] private GameObject _energyMenu;
    [SerializeField] private FeatherTracker _featherTimer;

    private void Start()
    {
        UpdateEnergyCount();
        StartCoroutine(EnergyDisplayLoop());
    }

    public void UpdateEnergyCount()
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long last = SaveSystem.instance.currency.playerCurrencyLastLogin;

        long elapsed = now - last;
        int energyToAdd = (int)(elapsed / _regenInterval);

        if (energyToAdd > 0)
        {
            SaveService.instance.ModifyEnergy(energyToAdd);

            long newLast = last + (energyToAdd * _regenInterval);
            SaveService.instance.ChangeLastLogin(newLast);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveLast();
    }

    private void OnApplicationQuit()
    {
        SaveLast();
    }

    private void SaveLast()
    {
        SaveService.instance.ChangeLastLogin(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }

    public void ToggleEnergyMenu()
    {
        _energyMenu.SetActive(!_energyMenu.activeInHierarchy);
    }

    public void UpdateTimeLeft()
    {
        _featherTimer.TimeLeft(SaveSystem.instance.currency.playerCurrencyLastLogin, _regenInterval);
    }

    IEnumerator EnergyDisplayLoop()
    {
        while (true)
        {
            UpdateEnergyCount();

            SaveSystem.instance.CallDataUpdate();

            if (_energyMenu.activeInHierarchy)
                _featherTimer.TimeLeft(SaveSystem.instance.currency.playerCurrencyLastLogin, _regenInterval);

            yield return new WaitForSeconds(1f);
        }
    }
}
