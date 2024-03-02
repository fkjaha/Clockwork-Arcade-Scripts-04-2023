using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image mainImage;
    [SerializeField] private CurrencyIconUI currencyIconUI;
    [SerializeField] private Button button;
    [SerializeField] private MMF_Player pressFeedback;

    private PurchaseHandler _purchaseHandler;
    private int _index;
    private Shop _shop;

    public void Initialize(Shop shop, int index)
    {
        button.onClick.AddListener(() =>
        {
            shop.TryUpgradeAtIndex(index);
            UpdateCostText();
            UpdateButtonState();
            pressFeedback.PlayFeedbacks();
        });
        _index = index;
        _shop = shop;
    }

    public void UpdateVisuals()
    {
        if (_shop.GetNumberOfOptions <= _index)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        UpgradableInfo info = _shop.GetOptionInfo(_index);
        titleText.text = info.GetLabel;
        mainImage.sprite = info.GetSprite;
        currencyIconUI.Initialize(_shop.GetOptionCurrencyType(_index));
        UpdateCostText();
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        button.interactable = _shop.CanUpgradeAtIndex(_index);
    }

    private void UpdateCostText()
    {
        int cost = _shop.GetOptionCost(_index);
        costText.text = "" + (cost == Int32.MaxValue ? "MAX" : cost);
    }
}
