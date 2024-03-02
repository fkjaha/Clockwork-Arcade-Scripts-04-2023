using UnityEngine;

public class UpgradableInt : AUpgradable
{
    public int GetInt => _currentValue;
    
    [SerializeField] private int startValue;
    [SerializeField] private int valuePerLevel;

    private int _currentValue;
    
    private protected override void ApplyLevel()
    {
        _currentValue = startValue + level * valuePerLevel;
    }
}
