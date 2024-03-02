using UnityEngine;
using UnityEngine.Events;

public abstract class PCCore : AUpgradable
{
    public event UnityAction OnBooted;

    public bool IsBooted => _booted;
    private protected Transform ExecuteAreaTransform => executeArea.transform;

    [Header("PCCore")]
    [SerializeField] private UpgradeArea executeArea;
    [SerializeField] private GameObject lockedArea;
    
    private bool _booted;


    private protected override void Start()
    {
        base.Start();
        HideActivationButton();
        SubscribeConnectedMachines();
    }

    private protected abstract void SubscribeConnectedMachines();

    private protected override void ApplyLevel()
    {
        if (level > 0)
        {
            Boot();
        }
    }

    private void Boot()
    {
        _booted = true;
        OnBooted?.Invoke();
    }

    private protected void CheckForFullConnection()
    {
        if(!AllMachinesConnected()) return;
        ShowActivationButton();
    }

    private protected abstract bool AllMachinesConnected();

    private void ShowActivationButton()
    {
        executeArea.EnableArea();
        lockedArea.SetActive(false);
        OnActivationShowed();
    }
    
    private void HideActivationButton()
    {
        executeArea.DisableArea();
        lockedArea.SetActive(true);
        OnActivationHided();
    }

    private protected virtual void OnActivationShowed() { }
    
    private protected virtual void OnActivationHided() { }
}
