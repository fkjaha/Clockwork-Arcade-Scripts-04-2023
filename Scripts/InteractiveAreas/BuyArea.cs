using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BuyArea : MonoBehaviour
{
    public event UnityAction OnUpgraded;
    public event UnityAction OnUpdated;

    public int GetGatherTarget => _moneyGatherTarget;
    public float GetUpgradeProgress => _timePassed / timeBeforeUpgrade;
    
    [SerializeField] private PlayerArea playerArea;
    [SerializeField] private float timeBeforeUpgrade;

    private bool _enabled;
    private float _timePassed;
    private PurchaseHandler _purchaseHandler;
    private bool _gatheringActive;
    private int _moneyGatherTarget;
    private CurrencyType _currencyType;

    public void Initialize(int gatherTarget, CurrencyType currencyType)
    {
        _moneyGatherTarget = gatherTarget;
        _currencyType = currencyType;
        OnUpdated?.Invoke();
    }

    private void Start()
    {
        playerArea.OnPlayerEntered += () => _gatheringActive = true;
        playerArea.OnPlayerLeft += () =>
        {
            _gatheringActive = false;
            OnUpdated?.Invoke();
        };
        _purchaseHandler = Currencies.Instance.GetCurrencyInfo(_currencyType).GetPurchaseHandler;
        // UpdateTarget();
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
        OnUpgraded?.Invoke();
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        OnUpdated?.Invoke();
    }
    
    public void EnableArea()
    {
        gameObject.SetActive(true);
        UpdateTarget();
    }
    
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
