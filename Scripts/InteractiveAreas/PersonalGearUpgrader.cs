using UnityEngine;
using UnityEngine.Events;

public class PersonalGearUpgrader : MonoBehaviour
{
    public event UnityAction OnUsed;
    
    [SerializeField] private ChildGear gear;
    [SerializeField] private BuyArea buyArea;

    public void Initialize(ChildGear targetGear, int cost, CurrencyType currencyType)
    {
        gear = targetGear;
        buyArea.Initialize(cost, currencyType);
        buyArea.OnUpgraded += BuyGear;
    }

    private void BuyGear()
    {
        gear.EnableGearFunctionality();
        OnUsed?.Invoke();
    }
}
