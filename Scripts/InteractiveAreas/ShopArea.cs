using System.Collections.Generic;
using UnityEngine;

public class ShopArea : MonoBehaviour
{
    public List<AUpgradable> GetUpgradables => upgradables;
    
    [SerializeField] private List<AUpgradable> upgradables;
    [SerializeField] private PlayerArea playerArea;
    
    private void Start()
    {
        playerArea.OnPlayerEntered += () => Shop.Instance.ShopAreaEntered(this);
        playerArea.OnPlayerLeft += () => Shop.Instance.ShopAreaLeft(this);
    }
}
