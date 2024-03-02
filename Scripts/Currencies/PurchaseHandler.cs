using UnityEngine;

public class PurchaseHandler : MonoBehaviour
{
    [SerializeField] private CurrencyHolder currencyHolder;
    
    public bool TryPurchase(int cost)
    {
        return currencyHolder.TryChangeCurrencyNumber(-cost);
    }

    public bool CanPurchase(int cost)
    {
        return currencyHolder.GetCurrencyAmount >= cost;
    }
}
