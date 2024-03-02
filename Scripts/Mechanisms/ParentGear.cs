using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParentGear : Gear
{
    public List<Gear> GetChildGears => connectedGears;

    [SerializeField] private List<Gear> connectedGears;

    private float _currentRotationLenght;
    private float _currentRotationTime;
    
    private protected override void Start()
    {
        base.Start();
        foreach (Gear connectedGear in connectedGears)
        {
            connectedGear.OnEnabled += () =>
            {
                if(!IsActive) return;
                float multiplier = 1 - GetRotationProgress;
                // if(Mathf.Abs(multiplier) < 0.001f) return;
                if(Math.Abs(GetRotationProgress - 1) < .0001f) return;
                connectedGear.RotateLenght(-_currentRotationLenght * multiplier,
                    _currentRotationTime * multiplier);
            };
        }
    }

    private protected void RotateChildren()
    {
        foreach (Gear connectedGear in connectedGears)
        {
            connectedGear.RotateLenght(-_currentRotationLenght, _currentRotationTime);
        }
    }

    public override void RotateLenght(float lenght, float time, UnityAction onCompleted = null)
    {
        if(!IsActive) return;
        base.RotateLenght(lenght, time, onCompleted);
        _currentRotationLenght = lenght;
        _currentRotationTime = time;
        RotateChildren();
    }

    public override void StopRotation()
    {
        base.StopRotation();
        foreach (Gear connectedGear in connectedGears)
        {
            connectedGear.StopRotation();
        }
    }

    public void AddChild(Gear childGear)
    {
        connectedGears.Add(childGear);
    }
}
