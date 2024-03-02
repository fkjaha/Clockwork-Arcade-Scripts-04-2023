using UnityEngine;

public class OneTimeUpgradeArea : MonoBehaviour
{
    [SerializeField] private UpgradeArea upgradeArea;

    private void Start()
    {
        upgradeArea.OnUpgraded += upgradeArea.DisableArea;
    }
}
