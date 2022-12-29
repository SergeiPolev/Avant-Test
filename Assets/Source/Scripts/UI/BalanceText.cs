using TMPro;
using UnityEngine;

public class BalanceText : MonoBehaviour
{
    private TextMeshProUGUI moneyText;

    private void Awake()
    {
        moneyText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateMoneyText(float value)
    {
        moneyText.text = $"Баланс: {value}$";
    }
}