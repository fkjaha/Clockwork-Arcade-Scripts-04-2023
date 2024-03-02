using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currencies : MonoBehaviour
{
    public static Currencies Instance;

    [SerializeField] private List<CurrencyInfo> currencyInfos;

    public CurrencyInfo GetCurrencyInfo(CurrencyType currencyType)
    {
        return currencyInfos.Find(info => info.GetCurrencyType == currencyType);
    }

    private void Awake()
    {
        Instance = this;
    }
}

[Serializable]
public class CurrencyInfo
{
    public CurrencyType GetCurrencyType => currencyCurrencyType;
    public CurrencyHolder GetCurrencyHolder => currencyHolder;
    public PurchaseHandler GetPurchaseHandler => purchaseHandler;
    public IncomeHandler GetIncomeHandler => incomeHandler;
    public Sprite GetSprite => sprite;
    public Material GetMaterial => material;

    [SerializeField] private CurrencyType currencyCurrencyType;
    [Space(20f)]
    [SerializeField] private CurrencyHolder currencyHolder;
    [SerializeField] private PurchaseHandler purchaseHandler;
    [SerializeField] private IncomeHandler incomeHandler;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Material material;
}

public enum CurrencyType
{
    Money,
    Energy
}
