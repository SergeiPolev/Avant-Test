using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Game/Business/Info")]
public class BusinessInfo : ScriptableObject
{
    [field: SerializeField] public string BusinessName { get; set; }
    [field: SerializeField] public string FirstUpgradeName { get; set; }
    [field: SerializeField] public string SecondUpgradeName { get; set; }
}