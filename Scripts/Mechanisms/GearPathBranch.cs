using System.Collections.Generic;
using UnityEngine;

public class GearPathBranch : MonoBehaviour
{
    [SerializeField] private GearPath gearPath;
    [SerializeField] private List<UpgradeArea> branches;

    private void Start()
    {
        gearPath.OnPathCompleted += () =>
        {
            foreach (UpgradeArea upgradeArea in branches)
            {
                upgradeArea.EnableArea();
            }
        };
    }
}
