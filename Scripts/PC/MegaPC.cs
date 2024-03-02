using System.Collections.Generic;
using UnityEngine;

public class MegaPC : PCCore
{
    [Header("MegaPC")]
    [SerializeField] private List<PC> connectedPcs;
    [Header("Attention")]
    [SerializeField] private CameraTargetController cameraTargetController;
    [SerializeField] private float cameraAttentionTime;
    [SerializeField] private GameObject attentionSign;

    private protected override void SubscribeConnectedMachines()
    {
        foreach (PC connectedMachine in connectedPcs)
        {
            connectedMachine.OnBooted += CheckForFullConnection;
        }
    }

    private protected override bool AllMachinesConnected()
    {
        foreach (PC connectedMachine in connectedPcs)
        {
            if(!connectedMachine.IsBooted) return false;
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
