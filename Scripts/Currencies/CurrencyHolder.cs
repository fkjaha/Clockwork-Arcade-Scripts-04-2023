using UnityEngine;
using UnityEngine.Events;

public class CurrencyHolder : MonoBehaviour
{
    public event UnityAction OnCurrencyUpdated;

    public int GetCurrencyAmount => _currency;

    [SerializeField] private int startCurrencyAmount;
    
    private int _currency;

    private void Start()
    {
        _currency = startCurrencyAmount;
    }

    public bool TryChangeCurrencyNumber(int delta)
    {
        if (_currency + delta < 0) return false;
        
        _currency += delta;
        OnCurrencyUpdated?.Invoke();
        return true;
    }
}
