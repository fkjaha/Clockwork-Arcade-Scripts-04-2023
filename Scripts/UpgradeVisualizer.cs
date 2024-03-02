using System;
using UnityEngine;

public class UpgradeVisualizer : MonoBehaviour
{
    [SerializeField] private UpgradeArea upgradeArea;

    private void Start()
    {
        if(upgradeArea != null)
            upgradeArea.OnUpgraded += () => UpgradeEffect.Instance.PlayUpgradeEffect(upgradeArea);
    }
}
