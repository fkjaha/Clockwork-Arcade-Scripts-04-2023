using UnityEngine;

public class PCCableVisualizer : CableStateVisualizer
{
    [Header("PCCore")]
    [SerializeField] private PCCore pcCore;

    private protected override void SubscribeToEnableEvent()
    {
        pcCore.OnBooted += ApplyRightMaterial;
    }

    private protected override bool ConnectedStuffEnabled()
    {
        return pcCore.IsBooted;
    }
}
