using UnityEngine;

public class GearRotationTime : AUpgradable
{
    public float GetSpeed => _speed;
    
    [SerializeField] private float startSpeed;
    [SerializeField] private float percentPerLevel;

    private float _speed;
    
    private protected override void ApplyLevel()
    {
        _speed = startSpeed * Mathf.Pow(percentPerLevel, level - 1);
    }
}
