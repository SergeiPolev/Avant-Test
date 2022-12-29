using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BusinessLevel
{
    public BusinessLevel(BusinessStats stats, int level, List<BusinessUpgrade> upgrades, float incomeProgress)
    {
        Stats = stats;
        Level = level;
        Upgrades = upgrades;
        IncomeProgress = incomeProgress;
    }

    public BusinessStats Stats;
    public int Level;
    public List<BusinessUpgrade> Upgrades;
    public float IncomeProgress;
}

[Serializable]
public struct BusinessLevelSerialize
{
    public BusinessLevelSerialize(string id, int level, string[] upgrades, float incomeProgress)
    {
        ID = id;
        Level = level;
        Upgrades = upgrades;
        IncomeProgress = incomeProgress;
    }

    public string ID;
    public int Level;
    public string[] Upgrades;
    public float IncomeProgress;
}

[Serializable]
public struct BusinessLevelSerializeArray
{
    public List<BusinessLevelSerialize> BusinessLevels;
}

public class BusinessesDatabase
{
    private const string KEY = "GAME_SAVE";
    private const string RESOURCE_PATH_BUSINESS = "Business";
    private const string RESOURCE_PATH_UPGRADES = "Business Upgrades";

    private Dictionary<string, BusinessLevel> businesses;

    public BusinessesDatabase()
    {
        InitDatabase();
    }

    #region DB Initialize
    private void InitDatabase()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            LoadDB();

            return;
        }

        var stats = Resources.LoadAll<BusinessStats>(RESOURCE_PATH_BUSINESS);
        businesses = new Dictionary<string, BusinessLevel>();

        foreach (var item in stats)
        {
            businesses.Add(item.ID, new BusinessLevel(item, item.DefaultBought ? 1 : 0, new List<BusinessUpgrade>(), 0));
        }

        SaveDB();
    }

    private void SaveDB()
    {
        if (businesses != null)
        {
            List<BusinessLevelSerialize> savings = new List<BusinessLevelSerialize>();

            foreach (var item in businesses)
            {
                List<string> upgradesList = new List<string>();

                foreach (var upgrade in item.Value.Upgrades)
                {
                    upgradesList.Add(upgrade.ID);
                }

                var business = new BusinessLevelSerialize(item.Key, GetLevel(item.Key), upgradesList.ToArray(), item.Value.IncomeProgress);

                savings.Add(business);
            }

            var listBusiness = new BusinessLevelSerializeArray();
            listBusiness.BusinessLevels = savings;

            string str = JsonUtility.ToJson(listBusiness);

            PlayerPrefs.SetString(KEY, str);
        }
    }

    private void LoadDB()
    {
        var savings = JsonUtility.FromJson<BusinessLevelSerializeArray>(PlayerPrefs.GetString(KEY));

        var stats = Resources.LoadAll<BusinessStats>(RESOURCE_PATH_BUSINESS).ToList();
        var upgrades = Resources.LoadAll<BusinessUpgrade>(RESOURCE_PATH_UPGRADES).ToList();

        businesses = new Dictionary<string, BusinessLevel>();

        foreach (var item in savings.BusinessLevels)
        {
            List<BusinessUpgrade> upgradesList = new List<BusinessUpgrade>();

            foreach (var upgrade in item.Upgrades)
            {
                var upgradeSO = upgrades.First(x => x.ID == upgrade);
                upgradesList.Add(upgradeSO);
            }

            BusinessStats stat = stats.First(x => x.ID == item.ID);

            var business = new BusinessLevel(stat, item.Level, upgradesList, item.IncomeProgress);

            businesses.Add(item.ID, business);
        }
    }
    private void CheckForInit()
    {
        if (businesses == null)
        {
            InitDatabase();
        }
    }
    #endregion
    #region DB Management
    // Setters
    public void IncreaseLevel(string id)
    {
        CheckForInit();

        businesses[id].Level++;

        SaveDB();
    }
    public void SetIncomeProgress(string id, float value)
    {
        CheckForInit();

        businesses[id].IncomeProgress = value;

        SaveDB();
    }
    public void AddUpgrade(string id, BusinessUpgrade upgrade)
    {
        CheckForInit();

        businesses[id].Upgrades.Add(upgrade);

        SaveDB();
    }

    // Getters
    public int GetLevel(string id)
    {
        CheckForInit();

        return businesses[id].Level;
    }
    public BusinessStats GetStats(string id)
    {
        CheckForInit();

        return businesses[id].Stats;
    }
    public List<BusinessUpgrade> GetUpgrades(string id)
    {
        CheckForInit();

        return businesses[id].Upgrades;
    }
    public Dictionary<string, BusinessLevel> GetBusinesses()
    {
        CheckForInit();

        return businesses;
    }
    public bool HaveUpgrade(string id, BusinessUpgrade upgrade)
    {
        CheckForInit();

        return businesses[id].Upgrades.Contains(upgrade);
    }
    public float GetIncomeProgress(string id)
    {
        CheckForInit();

        return businesses[id].IncomeProgress;
    }
    #endregion
}