using System.Collections.Generic;
using UnityEngine;

public class PersonalGearUpraderSpawner : MonoBehaviour
{
    [SerializeField] private List<GearPath> paths;
    [SerializeField] private MainGear mainGear;
    [SerializeField] private CurrencyType currencyType;
    [SerializeField] private int startCost;
    [SerializeField] private int costIncreasePerLevel;
    [SerializeField] private PersonalGearUpgrader personalGearUpgraderPrefab;

    private void Awake()
    {
        SpawnAndInitializePersonalUpgraders();
    }

    private void SpawnAndInitializePersonalUpgraders()
    {
        // foreach (GearPath gearPath in paths)
        // {
        //     foreach (ChildGear childGear in gearPath.GetPathGears)
        //     {
        //         SpawnAndInitializeSingleUpgrader(gearPath, childGear);
        //     }
        // }

        foreach (Gear startGear in mainGear.GetChildGears)
        {
            if(startGear is ChildGear childGear)
                SpawnAndInitializeSingleUpgrader(childGear, startCost);
        }
    }
    
    private void SpawnAndInitializeSingleUpgrader(ChildGear childGear, int cost)
    {
        PersonalGearUpgrader personalGearUpgraderInstance =
            Instantiate(personalGearUpgraderPrefab, childGear.transform.position.Vector3ToFlat(), Quaternion.identity);
        personalGearUpgraderInstance.OnUsed += () =>
        {
            foreach (Gear gear in childGear.GetChildGears)
            {
                if (!gear.GetIsEnabled && !(gear is PowerGear))
                {
                    SpawnAndInitializeSingleUpgrader((ChildGear)gear, cost + costIncreasePerLevel);
                }
            }
        };
        personalGearUpgraderInstance.Initialize(childGear, cost, currencyType);
    }

    // private void SpawnAndInitializeSingleUpgrader(GearPath gearPath, ChildGear childGear)
    // {
    //     PersonalGearUpgrader personalGearUpgraderInstance =
    //         Instantiate(personalGearUpgraderPrefab, childGear.transform.position.Vector3ToFlat(), Quaternion.identity);
    //     personalGearUpgraderInstance.Initialize(gearPath, childGear);
    // }
}
