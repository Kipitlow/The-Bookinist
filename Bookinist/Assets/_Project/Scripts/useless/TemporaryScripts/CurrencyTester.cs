using UnityEngine;

public class CurrencyTester : MonoBehaviour
{
    public void AddSoft() => CurrencyManager.Instance.AddSoftCurrency(50);
    public void SpendSoft() => CurrencyManager.Instance.SpendSoftCurrency(30);
    public void AddHard() => CurrencyManager.Instance.AddHardCurrency(10);
}