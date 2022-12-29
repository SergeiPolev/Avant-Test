using System;
using UnityEngine;

public class MoneyManager
{
    private const string MONEY_SAVE_KEY = "Money";

    public event Action<float> OnMoneyChanged;

    public float GetMoney()
    {
        return PlayerPrefs.GetFloat(MONEY_SAVE_KEY, 0);
    }
    public void Buy(float amount)
    {
        var currentMoney = PlayerPrefs.GetFloat(MONEY_SAVE_KEY, 0);

        if (currentMoney <= 0)
        {
            Debug.LogError("CANNOT DECREASE BELOW ZERO");

            return;
        }

        currentMoney -= amount;
        OnMoneyChanged?.Invoke(currentMoney);

        PlayerPrefs.SetFloat(MONEY_SAVE_KEY, currentMoney);
    }
    public void IncreaseMoney(float amount)
    {
        var currentMoney = PlayerPrefs.GetFloat(MONEY_SAVE_KEY, 0);

        currentMoney += amount;
        OnMoneyChanged?.Invoke(currentMoney);

        PlayerPrefs.SetFloat(MONEY_SAVE_KEY, currentMoney);
    }
}