using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class AUpgradable: MonoBehaviour
{
    public event UnityAction OnLevelUpgraded;
    public event UnityAction OnInitializedFields;
    
    public int GetLevel => level;
    public int GetCost => _cost;
    public string GetTitle => info.GetLabel;
    public bool GetMaxLevelReached => _maxLevelReached;
    public CurrencyType GetCurrencyType => currencyType;
    public UpgradableInfo GetInfo => info;


    [Header("Info")]
    [SerializeField] private protected int level;
    [Header("Variables")]
    [SerializeField] private UnityEvent onLevelUpgradedEvent;
    [SerializeField] private CurrencyType currencyType;
    [SerializeField] private UpgradableInfo info;
    [Space(10f)]
    [SerializeField] private int startCost;
    [SerializeField] private int costIncreasePerLevel;
    [SerializeField] private int startLevel;
    [SerializeField] private bool hasMaxLevel;
    [SerializeField] private int maxLevel;
    [Space(10f)]
    [SerializeField] private string levelSaveKey;

    private int _cost;
    private bool _maxLevelReached;

    private protected virtual void Start()
    {
        ApplyLevel();
        UpdateCost();
        OnInitializedFields?.Invoke();
        OnLevelUpgraded?.Invoke();
    }

    public void UpgradeLevelByOne()
    {
        level++;
        onLevelUpgradedEvent.Invoke();
        UpdateCost();
        ApplyLevel();
        OnLevelUpgraded?.Invoke();
    }

    private void UpdateCost()
    {
        _cost = (!hasMaxLevel || level < maxLevel) ? (startCost + level * costIncreasePerLevel) : Int32.MaxValue;
        // Debug.Log(_cost, this);
    }
    
    private protected abstract void ApplyLevel();

    private protected virtual void MaxLevelReached()
    {
        _maxLevelReached = true;
    }

    public void SaveLevel()
    {
        PlayerPrefs.SetInt(levelSaveKey, level);
    }

    public void LoadLevel()
    {
        level = PlayerPrefs.GetInt(levelSaveKey, startLevel);
        UpdateCost();
        ApplyLevel();
    }

    public void ResetLevel()
    {
        PlayerPrefs.DeleteKey(levelSaveKey);
        LoadLevel();
    }
}