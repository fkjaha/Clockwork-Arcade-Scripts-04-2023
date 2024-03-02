using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class GearGridCell : MonoBehaviour
{
    public event UnityAction OnRotate;
    public event UnityAction OnUpdated;
    
    [SerializeField] private Gear gear;

    public bool IsFilled()
    {
        return gear != null;
    }

    public bool TryFillCell(Gear content)
    {
        if (IsFilled()) return false;
        gear = content;
        gear.OnRotationCalled += Rotate;
        Updated();
        return true;
    }

    public Gear GetGear()
    {
        return gear;
    }

    public Gear EmptyCell()
    {
        Gear resultGear = gear;
        gear.OnRotationCalled -= Rotate;
        gear = null;
        Updated();
        return resultGear;
    }

    private void Rotate()
    {
        OnRotate?.Invoke();
    }

    private void Updated()
    {
        OnUpdated?.Invoke();
    }
}
