using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GearIncomeSubscriber : MonoBehaviour
{
    public event UnityAction OnIncomePerSecondUpdated;
    
    public int GetIncome => incomePerRotation.GetInt;
    public float GetIncomePerSecond => _incomePerSecond;

    [SerializeField] private GearRotationTime mainGearRotationTime;
    [SerializeField] private CurrencyType incomeType;
    [SerializeField] private UpgradableInt incomePerRotation;

    private IncomeHandler _incomeHandler;
    private float _incomePerSecond;
    private int _numberOfActiveGears;
    
    private void Start()
    {
        _incomeHandler = Currencies.Instance.GetCurrencyInfo(incomeType).GetIncomeHandler;
        SubscribeAllEvents();
        UpdateIncomePerSecond();
    }

    private protected virtual void SubscribeAllEvents()
    {
        mainGearRotationTime.OnLevelUpgraded += UpdateIncomePerSecond;
        SubscribeAllGearsIncome();
    }

    private protected virtual void SubscribeAllGearsIncome()
    {
        foreach (Gear gear in FindObjectsOfType<Gear>())
        {
            if(gear is PowerGear) continue;
            SubscribeSingleGearIncome(gear);
            gear.OnEnabled += () =>
            {
                _numberOfActiveGears++;
                UpdateIncomePerSecond();
            };
        }
    }

    private protected void SubscribeSingleGearIncome(Gear gear, int incomeMultiplier = 1)
    {
        gear.OnRotationCalled += () =>
            {
                _incomeHandler.GetIncome(incomePerRotation.GetInt * incomeMultiplier);
            };
    }

    private void UpdateIncomePerSecond()
    {
        _incomePerSecond = mainGearRotationTime.GetLevel > 0 ?
                               (int) (((_numberOfActiveGears + 1) * incomePerRotation.GetInt / mainGearRotationTime.GetSpeed) * 10) /
                           10f : 0;
        OnIncomePerSecondUpdated?.Invoke();
    }
}
