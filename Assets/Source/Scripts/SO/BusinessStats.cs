using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Game/Business/Stats")]
public class BusinessStats : ScriptableObject
{
    [field: SerializeField] public string ID { get; set; }
    [field: SerializeField] public BusinessInfo BusinessInfo { get; set; }
    [field: SerializeField] public float IncomeDelay { get; set; }
    [field: SerializeField] public float BaseCost { get; set; }
    [field: SerializeField] public float BaseIncome { get; set; }
    [field: SerializeField] public BusinessUpgrade FirstUpgrade { get; set; }
    [field: SerializeField] public BusinessUpgrade SecondUpgrade { get; set; }
    [field: SerializeField] public bool DefaultBought { get; set; }

    public bool CanAffordLevelUp(float balance, int currentLevel)
    {
        return balance >= GetLevelUpCost(currentLevel);
    }
    public float GetLevelUpCost(int currentLevel)
    {
        return (currentLevel + 1) * BaseCost;
    }
    public float GetIncome(int currentLevel, bool firstAvailable, bool secondAvailable)
    {
        var firstMultiplier = firstAvailable ? FirstUpgrade.GetCalcilatedValue() : 0;
        var secondMultiplier = secondAvailable ? SecondUpgrade.GetCalcilatedValue() : 0;

        return currentLevel * BaseIncome * (1 + firstMultiplier + secondMultiplier);
    }
}