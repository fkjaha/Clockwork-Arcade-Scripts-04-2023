using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GearPath : AUpgradable
{
    public event UnityAction OnPathCompleted;

    public List<ChildGear> GetPathGears => pathGears;

    [SerializeField] private UnityEvent onPathCompleted;
    [SerializeField] private List<ChildGear> pathGears;
    [SerializeField] private Transform gearsParent;
    
    private protected override void ApplyLevel()
    {
        for (int i = 0; i < pathGears.Count; i++)
        {
            // pathGears[i].gameObject.SetActive(level - 1 >= i);
            // if(level - 1 >= i)
            //     pathGears[i].EnableFunctions();
        }
        if(level - 1 >= pathGears.Count-1) MaxLevelReached();
    }

    public void AddToPath(ChildGear childGear)
    {
        childGear.transform.parent = gearsParent;
        if(pathGears.Count > 0)
            pathGears[^1].AddChild(childGear);
        pathGears.Add(childGear);
    }

    public bool ContainsGear(ChildGear gearTarget)
    {
        return pathGears.Contains(gearTarget);
    }

    public void ClearPath()
    {
        pathGears = new();
        foreach (ChildGear content in gearsParent.GetComponentsInChildren<ChildGear>())
        {
            GameObject destroyTarget = content.gameObject;
            DestroyImmediate(destroyTarget);
        }
    }

    private protected override void MaxLevelReached()
    {
        base.MaxLevelReached();
        OnPathCompleted?.Invoke();
        onPathCompleted.Invoke();
    }
}
