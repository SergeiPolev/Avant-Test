using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialUpgradeButtonUI : MonoBehaviour
{
    [SerializeField] private Button specialUpgradeButton;
    [SerializeField] private TextMeshProUGUI specialUpgradeText;

    private BusinessesDatabase db;
    private BusinessUpgrade upgrade;
    private BusinessStats stats;

    public event Action<float> OnUpgrade;

    private string showName;

    public void InitButton(BusinessesDatabase database, BusinessUpgrade upgrade, BusinessStats stats, string showName)
    {
        db = database;
        this.upgrade = upgrade;
        this.stats = stats;
        this.showName = showName;

        specialUpgradeButton.onClick.AddListener(Upgrade);

        SetText(db.HaveUpgrade(stats.ID, upgrade));
    }
    public void SetAvailability(bool enable)
    {
        specialUpgradeButton.interactable = enable;
    }
    public void SetText(bool isBought = false)
    {
        string endLine = isBought ? "Куплено" : $"Цена: { upgrade.Price }$";
        specialUpgradeText.text = $"\"{showName}\"\nДоход: +{ upgrade.Value }%\n" + endLine;
    }

    private void Upgrade()
    {
        db.AddUpgrade(stats.ID, upgrade);
        OnUpgrade?.Invoke(upgrade.Price);
    }
}