using UnityEngine;

public class MachineCableVisualizer : CableStateVisualizer
{
    [Header("Machine")]
    [SerializeField] private MoneyMachine moneyMachine;

    private protected override void SubscribeToEnableEvent()
    {
        moneyMachine.OnFirstUse += ApplyRightMaterial;
    }

    private protected override bool ConnectedStuffEnabled()
    {
        return  moneyMachine.FirstUseRegistered;
    }
}
