using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeAreaVisualizer : MonoBehaviour
{
    [SerializeField] private UpgradeArea upgradeArea;

    [SerializeField] private Image progressImage;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI nameText;


    private void Awake()
    {
        upgradeArea.OnUpdated += UpdateText;
    }

    private void UpdateText()
    {
        costText.text = "" + upgradeArea.GetGatherTarget;
        nameText.text = upgradeArea.GetTitle;
        progressImage.fillAmount = upgradeArea.GetUpgradeProgress;
    }
}
