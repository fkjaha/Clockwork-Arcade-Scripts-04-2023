using UnityEngine;

public class CurrencyIcon : MonoBehaviour
{
    [SerializeField] private MeshRenderer image;
    [SerializeField] private CurrencyType type;

    private void Start()
    {
        image.material = Currencies.Instance.GetCurrencyInfo(type).GetMaterial;
    }
}
