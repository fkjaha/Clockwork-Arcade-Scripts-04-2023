using UnityEngine;
using UnityEngine.Events;

public class MoneyMachine : AUpgradable
{
    public event UnityAction OnFirstUse;

    public bool FirstUseRegistered => !_executeForTheFirstTime;
    public int GetMoneyWorth => _moneyWorth;
    public int GetStacksPerRotation => _income;
    
    [SerializeField] private Gear chargingGear;
    [SerializeField] private MoneySpawner moneySpawner;
    [SerializeField] private GameObject dustObject;
    [Space(20f)]
    [SerializeField] private int startIncome;
    [SerializeField] private int incomePerLevel;
    [SerializeField] private int startMoneyWorth;
    [SerializeField] private int moneyWorthPerLevel;

    private bool _executeForTheFirstTime = true;
    private int _income;
    private int _moneyWorth;

    private protected override void Start()
    {
        base.Start();
        chargingGear.OnRotationEnded += Execute;
    }

    private protected override void ApplyLevel()
    {
        UpdateIncome();
    }

    private void Execute()
    {
        if (_executeForTheFirstTime)
        {
            _executeForTheFirstTime = false;
            dustObject.SetActive(true);
            OnFirstUse?.Invoke();
        }
        moneySpawner.AddSpawnAmount(_income);
    }
    
    private void UpdateIncome()
    {
        _income = startIncome + level * incomePerLevel;
        _moneyWorth = startMoneyWorth + moneyWorthPerLevel * level;
    }
}
