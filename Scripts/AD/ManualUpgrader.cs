using UnityEngine;

public class ManualUpgrader : MonoBehaviour
{
    [SerializeField] private AUpgradable upgradable;
    [SerializeField] private KeyCode upgradeInput;

    private void Update()
    {
        if (Input.GetKeyDown(upgradeInput))
        {
            ManualUpgrade();
        }
    }
    
    public void ManualUpgrade()
    {
        upgradable.UpgradeLevelByOne();
    }
}
