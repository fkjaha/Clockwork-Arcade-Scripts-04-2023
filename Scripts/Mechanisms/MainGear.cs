using System.Collections;
using UnityEngine;

public class MainGear : ParentGear
{
    [SerializeField] private InputDetector inputDetector;
    [Space(20f)]
    [SerializeField] private GearRotationTime timePerAutoRotation;
    [SerializeField] private float timePerManualRotation;
    [SerializeField] private int degreesPerRotation;
    [SerializeField] private bool disableManualRotation;

    private float _rotationLenght;
    private bool _rotatingManually;
    
    private protected override void Start()
    {
        EnableGearFunctionality();
        base.Start();
        CountRotationLenght();
        inputDetector.OnScreenTapped += MakeManualRotation;
        timePerAutoRotation.OnLevelUpgraded += () =>
        {
            StopRotation();
            StartCoroutine(AutoRotate());
        };
        StartCoroutine(AutoRotate());
    }

    [ContextMenu("Manual Rotation Iteration")]
    private void MakeManualRotation()
    {
        if(disableManualRotation) return;
        if(_rotatingManually) return;
        _rotatingManually = true;
        RotateLenght(_rotationLenght, timePerManualRotation, ManualRotationFinished);
    }
    
    private void ManualRotationFinished()
    {
        _rotatingManually = false;
    }

    private void MakeAutoRotation()
    {
        RotateLenght(_rotationLenght, timePerAutoRotation.GetSpeed, MakeAutoRotation);
    }
    
    private void CountRotationLenght()
    {
        _rotationLenght = degreesPerRotation / 360f * GetLenght;
    }

    private IEnumerator AutoRotate()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (timePerAutoRotation.GetLevel > 0)
            {
                MakeAutoRotation();
                break;
            }
        }
    }

    public override void StopRotation()
    {
        base.StopRotation();
        StopAllCoroutines();
        ManualRotationFinished();
    }
}
