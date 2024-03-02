using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyArea : MonoBehaviour
{
    public int GetMoneyCount => _moneyPrefabs.Count;
    public bool MaxCapacityReached => _moneyPrefabs.Count >= maxCapacity;

    [SerializeField] private CurrencyType incomeType;
    [SerializeField] private PlayerArea playerArea;
    [SerializeField] private float timeBetweenGiveaways;
    [SerializeField] private int maxCapacity;

    private IncomeHandler _incomeHandler;
    private readonly Stack<Money> _moneyPrefabs = new();
    private bool _giveawayEnabled;
    private int _giveawayAmount;
    
    private void Start()
    {
        _incomeHandler = Currencies.Instance.GetCurrencyInfo(incomeType).GetIncomeHandler;
        
        playerArea.OnPlayerEntered += () => _giveawayEnabled = true;
        playerArea.OnPlayerLeft += () => _giveawayEnabled = false;

        StartCoroutine(GiveawayCoroutine());
    }

    public void AddMoney(Money moneyObject)
    {
        _moneyPrefabs.Push(moneyObject);
    }

    private IEnumerator GiveawayCoroutine()
    {
        while (true)
        {
            if (_giveawayEnabled)
                _giveawayAmount = _moneyPrefabs.Count;
            if (_giveawayAmount > 0 && _moneyPrefabs.Count > 0)
            {
                GiveawayOne();
                yield return new WaitForSeconds(timeBetweenGiveaways);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void GiveawayOne()
    {
        Money target = _moneyPrefabs.Pop();
        CoinsAnimator.Instance.AnimateSingleImage(CanvasPositionCalculator.Instance.GetScreenPositionVector2(target.transform.position));
        _incomeHandler.GetIncome(target.GetWorth);
        Destroy(target.gameObject);
        _giveawayAmount--;
    }
    
    private void GiveawayAll()
    {
        for (int i = 0; i < _moneyPrefabs.Count; i++)
        {
            GiveawayOne();
        }
    }
}
