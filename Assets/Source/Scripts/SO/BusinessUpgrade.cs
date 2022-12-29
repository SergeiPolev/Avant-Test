using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Game/Business/Upgrade")]
public class BusinessUpgrade : ScriptableObject
{
    [field: SerializeField] public string ID { get; set; }
    [field: SerializeField] public float Price { get; set; }
    [field: SerializeField] public float Value { get; set; }

    public float GetCalcilatedValue()
    {
        return Value / 100f;
    }
    public bool CanAffordUpgrade(float balance)
    {
        return balance >= Price;
    }
}