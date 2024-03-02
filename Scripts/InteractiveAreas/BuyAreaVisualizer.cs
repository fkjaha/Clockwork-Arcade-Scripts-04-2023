using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyAreaVisualizer : MonoBehaviour
{
    [SerializeField] private BuyArea buyArea;

    [SerializeField] private Image progressImage;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI nameText;
    
    private void Awake()
    {
        buyArea.OnUpdated += UpdateText;
    }

    private void UpdateText()
    {
        costText.text = "" + buyArea.GetGatherTarget;
        progressImage.fillAmount = buyArea.GetUpgradeProgress;
    }
}
