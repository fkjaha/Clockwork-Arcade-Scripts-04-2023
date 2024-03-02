using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeArea : MonoBehaviour
{
    public event UnityAction OnUpgraded;
    public event UnityAction OnUpdated;

    public int GetGatherTarget => _moneyGatherTarget;
    public string GetTitle => upgradable.GetTitle;
    public float GetUpgradeProgress => _timePassed / timeBeforeUpgrade;
    
    [SerializeField] private PlayerArea playerArea;
    [SerializeField] private AUpgradable upgradable;
    [SerializeField] private float timeBeforeUpgrade;
    [SerializeField] private bool initOnAwake;

    private bool _enabled;
    private float _timePassed;
    private PurchaseHandler _purchaseHandler;
    private bool _gatheringActive;
    private int _moneyGatherTarget;

    private void Awake()
    {
        if(initOnAwake)
            Initialize(upgradable);
    }

    private void Initialize(AUpgradable targetUpgradable)
    {
        upgradable = targetUpgradable;
        upgradable.OnInitializedFields += UpdateTarget;
    }

    private void OnEnable()
    {
        if(upgradable == null) return;
        upgradable.OnInitializedFields += UpdateTarget;
    }
    
    private void Start()
    {
        playerArea.OnPlayerEntered += () =>
        {
            _gatheringActive = true;
        };
        playerArea.OnPlayerLeft += () =>
        {
            _gatheringActive = false;
            OnUpdated?.Invoke();
        };
        upgradable.OnLevelUpgraded += UpdateTarget;
    
        _purchaseHandler = Currencies.Instance.GetCurrencyInfo(upgradable.GetCurrencyType).GetPurchaseHandler;
        StartCoroutine(GatheringCoroutine());
    }

    private IEnumerator GatheringCoroutine()
    {
        _timePassed = 0;
        while (true)
        {
            if (_gatheringActive && _purchaseHandler.CanPurchase(_moneyGatherTarget))
            {
                _timePassed += Time.deltaTime;
                if (_timePassed >= timeBeforeUpgrade)
                {
                    if (TryGather())
                    {
                        PurchaseUpgrade();
                    }
                    _timePassed = 0;
                }
                OnUpdated?.Invoke();
            }
            else if(_timePassed > 0)
            {
                _timePassed = 0;
                OnUpdated?.Invoke();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private bool TryGather()
    {
        bool purchaseSuccessful = _purchaseHandler.TryPurchase(_moneyGatherTarget);
        return purchaseSuccessful;
    }

    private void PurchaseUpgrade()
    {
        TargetMoneyGathered();
    }

    private void TargetMoneyGathered()
    {
        upgradable.UpgradeLevelByOne();
        OnUpgraded?.Invoke();
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        if(upgradable.GetMaxLevelReached)
            DisableArea();
        _moneyGatherTarget = upgradable.GetCost;
        OnUpdated?.Invoke();
    }

    public void EnableArea()
    {
        gameObject.SetActive(true);
        UpdateTarget();
        StopAllCoroutines();
        StartCoroutine(GatheringCoroutine());
    }
    
    public void DisableArea()
    {
        gameObject.SetActive(false);
    }
}
