using UnityEngine;

public class Speed : AUpgradable
{
    public float GetSpeed => _speed;
    
    [SerializeField] private float startSpeed;
    [SerializeField] private float speedPerLevel;

    private float _speed;
    
    private protected override void ApplyLevel()
    {
        _speed = startSpeed + level * speedPerLevel;
    }
}
