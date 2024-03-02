using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private CurrencyType currencyType;
    [SerializeField] private TextMeshProUGUI renderText;

    private CurrencyHolder _currencyHolder;

    private void Start()
    {
        _currencyHolder = Currencies.Instance.GetCurrencyInfo(currencyType).GetCurrencyHolder;
        _currencyHolder.OnCurrencyUpdated += UpdateRendering;
        UpdateRendering();
    }

    private void UpdateRendering()
    {
        renderText.text = "" + _currencyHolder.GetCurrencyAmount;
    }
}
