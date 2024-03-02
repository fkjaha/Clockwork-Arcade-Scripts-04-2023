using System.Collections.Generic;
using UnityEngine;

public class PC : PCCore
{
    [Header("PC")]
    [SerializeField] private CameraTargetController cameraTargetController;
    [SerializeField] private float cameraAttentionTime;
    [SerializeField] private GameObject attentionSign;
    [SerializeField] private List<MoneyMachine> connectedMachines;

    private protected override void SubscribeConnectedMachines()
    {
        foreach (MoneyMachine connectedMachine in connectedMachines)
        {
            connectedMachine.OnFirstUse += CheckForFullConnection;
        }
    }

    private protected override bool AllMachinesConnected()
    {
        foreach (MoneyMachine connectedMachine in connectedMachines)
        {
            if(!connectedMachine.FirstUseRegistered) return false;
        }

        return true;
    }

    private protected override void OnActivationShowed()
    {
        if(attentionSign == null || cameraTargetController == null) return;
        attentionSign.SetActive(true);
        cameraTargetController.LookAtAndReturn(ExecuteAreaTransform.position, cameraAttentionTime);    
    }

    private protected override void OnActivationHided()
    {
        if(attentionSign == null) return;
        attentionSign.SetActive(false);
    }
}
