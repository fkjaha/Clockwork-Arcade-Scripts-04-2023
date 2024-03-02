using UnityEngine;

public class BuyVisaulizer : MonoBehaviour
{
    [SerializeField] private BuyArea buyArea;

    private void Start()
    {
        if(buyArea != null)
            buyArea.OnUpgraded += () => UpgradeEffect.Instance.PlayUpgradeEffect(buyArea);
    }
}
