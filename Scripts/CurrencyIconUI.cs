using UnityEngine;
using UnityEngine.UI;

public class CurrencyIconUI : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private bool initializeSelf;
    [SerializeField] private CurrencyType selfInitializeType;

    private void Start()
    {
        if(initializeSelf)
            Initialize(selfInitializeType);
    }

    public void Initialize(CurrencyType initCurrencyType)
    {
        image.sprite = Currencies.Instance.GetCurrencyInfo(initCurrencyType).GetSprite;
    }
}
