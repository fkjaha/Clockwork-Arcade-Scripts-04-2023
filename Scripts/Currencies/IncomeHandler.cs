using UnityEngine;

public class IncomeHandler : MonoBehaviour
{
    [SerializeField] private CurrencyHolder currencyHolder;

    public void GetIncome(int income)
    {
        if(income <= 0) return;
        currencyHolder.TryChangeCurrencyNumber(income);
    }
}
