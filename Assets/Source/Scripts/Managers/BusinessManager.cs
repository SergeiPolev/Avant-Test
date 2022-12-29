using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BusinessManager : MonoBehaviour
{
    [SerializeField] private BusinessItemUI businessUIPrefab;
    [SerializeField] private Transform prefabsRoot;

    private Dictionary<BusinessItemLogic, BusinessItemUI> businesses = new Dictionary<BusinessItemLogic, BusinessItemUI>();

    private BusinessesDatabase db;
    private MoneyManager moneyManager;

    private void Awake()
    {
        db = new BusinessesDatabase();
        moneyManager = new MoneyManager();

        foreach (var item in db.GetBusinesses())
        {
            var newItemUI = Instantiate(businessUIPrefab, prefabsRoot);
            newItemUI.InitItem(db, item.Value.Stats);

            newItemUI.OnUpgrade += OnUpgradeBought;

            BusinessItemLogic logic = new BusinessItemLogic(db, item.Value.Stats);
            logic.OnIncome += moneyManager.IncreaseMoney;

            businesses.Add(logic, newItemUI);
        }

        var balanceText = GetComponentInChildren<BalanceText>();

        balanceText.UpdateMoneyText(moneyManager.GetMoney());

        moneyManager.OnMoneyChanged += balanceText.UpdateMoneyText;
        moneyManager.OnMoneyChanged += CheckAvailability;

        CheckAvailability(moneyManager.GetMoney());
    }

    private void OnUpgradeBought(float value)
    {
        moneyManager.Buy(value);
    }

    private void Update()
    {
        foreach (var item in businesses)
        {
            if (!item.Key.IsBought())
            {
                continue;
            }
            
            item.Key.Tick();
            item.Value.UpdateSlider(item.Key.GetCurrentIncomeValue());
        }
    }

    private void CheckAvailability(float value)
    {
        foreach (var item in businesses)
        {
            var levelUp = item.Key.CanAffordLevelUp(value);
            var first = item.Key.CanUpgrade(value, item.Key.GetStats().FirstUpgrade);
            var second = item.Key.CanUpgrade(value, item.Key.GetStats().SecondUpgrade);

            item.Value.CheckAvailability(levelUp, first, second);
        }
    }

    private void OnDisable()
    {
        SaveProgress();
    }

    private void SaveProgress()
    {
        if (businesses == null)
        {
            Debug.LogError("NO BUSINESSES");
        }

        foreach (var item in businesses)
        {
            db.SetIncomeProgress(item.Key.GetStats().ID, item.Key.GetCurrentIncomeValue());
        }
    }
}