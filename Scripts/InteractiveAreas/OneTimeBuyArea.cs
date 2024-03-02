using UnityEngine;

public class OneTimeBuyArea : MonoBehaviour
{
    [SerializeField] private BuyArea buyArea;

    private void Start()
    {
        buyArea.OnUpgraded += buyArea.Disable;
    }
}
