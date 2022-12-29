using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI businessTitle;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI incomeText;
    [SerializeField] private Slider incomeSlider;

    [Header("Level Up Button")]
    [SerializeField] private TextMeshProUGUI levelUpPrice;
    [SerializeField] private Button levelUpButton;

    [Header("Special Upgrade Buttons")]
    [SerializeField] private SpecialUpgradeButtonUI firstSpecialUpgrade;
    [SerializeField] private SpecialUpgradeButtonUI secondSpecialUpgrade;

    private BusinessStats stats;
    private BusinessesDatabase db;

    public event Action<float> OnUpgrade;

    public void InitItem(BusinessesDatabase db, BusinessStats stats)
    {
        businessTitle.text = $"{stats.BusinessInfo.BusinessName}";

        this.db = db;
        this.stats = stats;

        firstSpecialUpgrade.InitButton(db, stats.FirstUpgrade, stats, stats.BusinessInfo.FirstUpgradeName);
        secondSpecialUpgrade.InitButton(db, stats.SecondUpgrade, stats, stats.BusinessInfo.SecondUpgradeName);

        UpdateInfo();

        Subscribe();
    }

    private void Subscribe()
    {
        levelUpButton.onClick.AddListener(LevelUp);

        firstSpecialUpgrade.OnUpgrade += (float value) => OnUpgrade.Invoke(value);
        secondSpecialUpgrade.OnUpgrade += (float value) => OnUpgrade.Invoke(value);
    }

    public void UpdateInfo()
    {
        var currentLevel = db.GetLevel(stats.ID);
        bool firstSkill = db.HaveUpgrade(stats.ID, stats.FirstUpgrade);
        bool secondSkill = db.HaveUpgrade(stats.ID, stats.SecondUpgrade);

        levelText.text = $"LVL\n{currentLevel}";
        levelUpPrice.text = $"LVL UP\nЦена: {stats.GetLevelUpCost(currentLevel)}$";
        incomeText.text = $"Доход\n{stats.GetIncome(currentLevel, firstSkill, secondSkill)}$";

        firstSpecialUpgrade.SetText(firstSkill);
        secondSpecialUpgrade.SetText(secondSkill);
    }

    public void UpdateSlider(float value)
    {
        incomeSlider.value = value;
    }

    public void CheckAvailability(bool levelUp, bool firstUp, bool secondUp)
    {
        levelUpButton.interactable = levelUp;
        firstSpecialUpgrade.SetAvailability(firstUp);
        secondSpecialUpgrade.SetAvailability(secondUp);

        UpdateInfo();
    }
    
    private void LevelUp()
    {
        var level = db.GetLevel(stats.ID);
        db.IncreaseLevel(stats.ID);

        OnUpgrade?.Invoke(stats.GetLevelUpCost(level));
    }
}