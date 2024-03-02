using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    [SerializeField] private List<GameObject> bounds;
    [SerializeField] private AUpgradable upgradable;

    public void DestroyBounds()
    {
        if(upgradable.GetLevel < 2) return;
        
        foreach (GameObject bound in bounds)
        {
            Destroy(bound);
        }
        Destroy(this);
    }
}
