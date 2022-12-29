using System;
using UnityEngine;

public class BusinessItemLogic
{
    private BusinessesDatabase db;
    private BusinessStats stats;

    private float currentTimer;

    public event Action<float> OnIncome;

    public BusinessItemLogic(BusinessesDatabase db, BusinessStats stats)
    {
        this.db = db;
        this.stats = stats;

        currentTimer = db.GetIncomeProgress(stats.ID) * stats.IncomeDelay;
    }

    // Getters
    public float GetCurrentIncomeValue() => currentTimer / stats.IncomeDelay;
    public int GetLevel() => db.GetLevel(stats.ID);
    public BusinessStats GetStats() => stats;
    public bool CanAffordLevelUp(float balance) => stats.CanAffordLevelUp(balance, db.GetLevel(stats.ID));
    public bool CanUpgrade(float balance, BusinessUpgrade upgrade) => balance >= upgrade.Price && !db.HaveUpgrade(stats.ID, upgrade);
    public bool HaveUpgrade(BusinessUpgrade upgrade) =>  db.HaveUpgrade(stats.ID, upgrade);
    public bool IsBought() => db.GetLevel(stats.ID) > 0;

    // Methods
    public void Tick()
    {
        currentTimer += Time.deltaTime;

        if (currentTimer >= stats.IncomeDelay)
        {
            GetIncome();
        }
    }

    private void GetIncome()
    {
        currentTimer = 0;

        var income = stats.GetIncome(GetLevel(), HaveUpgrade(stats.FirstUpgrade), HaveUpgrade(stats.SecondUpgrade));

        OnIncome?.Invoke(income);
    }
}