using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MoneySpawner : MonoBehaviour
{
    public event UnityAction OnIncomePerSecondUpdated;
    
    public float GetIncomePerSecond => _incomePerSecond;

    [SerializeField] private MoneyMachine moneyMachine;
    [SerializeField] private MoneyArea moneyArea;
    [SerializeField] private GridSpawner spawner;
    [SerializeField] private Money moneyPrefab;
    [SerializeField] private Transform moneyParent;
    [SerializeField] private Transform moneyOriginalPosition;
    [SerializeField] private float timeBetweenSpawn;
    [SerializeField] private int spawnNumber;
    
    private float _incomePerSecond;
    private float _income;


    [ContextMenu("Test")]
    private void Test()
    {
        AddSpawnAmount(spawnNumber);
    }
    
    private int _amountLeft;

    private void Start()
    {
        StartCoroutine(SpawnMoney());
        StartCoroutine(CountIncomePerSecond());
    }

    public void AddSpawnAmount(int amount)
    {
        _amountLeft += amount;
    }

    private IEnumerator SpawnMoney()
    {
        while (true)
        {
            if (_amountLeft > 0)
            {
                SpawnSingle();
                yield return new WaitForSeconds(timeBetweenSpawn);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void SpawnSingle()
    {
        _amountLeft--;
        // Money spawned = spawner.SpawnWithGrid(moneyPrefab, moneyArea.GetMoneyCount);
        
        if(moneyArea.MaxCapacityReached) return;
        
        Money spawned = spawner.SpawnWithGrid(moneyPrefab, moneyArea.GetMoneyCount);
        spawned.GetVisualsTransform.position = moneyOriginalPosition.position;
        spawned.GetVisualsTransform.DOLocalMove(Vector3.zero, timeBetweenSpawn);
        spawned.transform.parent = moneyParent;
        spawned.SetWorth(moneyMachine.GetMoneyWorth);
        moneyArea.AddMoney(spawned);

        _income += spawned.GetWorth;
    }

    private IEnumerator CountIncomePerSecond()
    {
        while (true)
        {
            _income = 0;
            yield return new WaitForSeconds(1);
            _incomePerSecond = _income;
            OnIncomePerSecondUpdated?.Invoke();
        }
    }
}
