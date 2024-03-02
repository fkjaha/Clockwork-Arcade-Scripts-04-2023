using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shop : MonoBehaviour
{
    public event UnityAction OnOptionsUpdated;
    public event UnityAction OnShopAreaEntered;
    public event UnityAction OnShopAreaLeft;
    public event UnityAction OnOptionPurchased;

    public static Shop Instance;

    public int GetNumberOfOptions => _upgradables.Count;

    private List<AUpgradable> _upgradables;
    private ShopArea _activeShopArea;

    private void Awake()
    {
        Instance = this;
    }

    public bool TryUpgradeAtIndex(int index)
    {
        if (Currencies.Instance.GetCurrencyInfo(_upgradables[index].GetCurrencyType).GetPurchaseHandler
            .TryPurchase(_upgradables[index].GetCost))
        {
            _upgradables[index].UpgradeLevelByOne();
            OnOptionPurchased?.Invoke();
            return true;
        }
        return false;
    }
    
    public bool CanUpgradeAtIndex(int index)
    {
        return Currencies.Instance.GetCurrencyInfo(_upgradables[index].GetCurrencyType).GetPurchaseHandler
            .CanPurchase(_upgradables[index].GetCost);
    }

    public int GetOptionCost(int index)
    {
        return _upgradables[index].GetCost;
    }

    public CurrencyType GetOptionCurrencyType(int index)
    {
        return _upgradables[index].GetCurrencyType;
    }
    
    public UpgradableInfo GetOptionInfo(int index)
    {
        return _upgradables[index].GetInfo;
    }

    private void UpdateShopOptions(List<AUpgradable> options)
    {
        _upgradables = options;
        OnOptionsUpdated?.Invoke();
    }

    public void ShopAreaEntered(ShopArea shopArea)
    {
        if(_activeShopArea != null) return;
        _activeShopArea = shopArea;
        UpdateShopOptions(shopArea.GetUpgradables);
        OnShopAreaEntered?.Invoke();
    }

    public void ShopAreaLeft(ShopArea shopArea)
    {
        if(_activeShopArea != shopArea) return;
        _activeShopArea = null;
        OnShopAreaLeft?.Invoke();
    }
}
