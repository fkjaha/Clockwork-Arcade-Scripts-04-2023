using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/UpgradableInfo", fileName = "newInfo", order = 0)]
public class UpgradableInfo : ScriptableObject
{
    public Sprite GetSprite => sprite;
    public string GetLabel => label;
    public string GetDescription => description;
    
    [SerializeField] private Sprite sprite;
    [SerializeField] private string label;
    [SerializeField] private string description;
}
