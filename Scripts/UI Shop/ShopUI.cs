using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Shop shop;
    [SerializeField] private ShopSlot slotPrefab;
    [SerializeField] private GameObject shopUIParent;
    [SerializeField] private Transform slotsParent;
    [SerializeField] private int maxSlotsCount;

    private List<ShopSlot> _slots;

    private void Start()
    {
        shop.OnOptionsUpdated += UpdateView;
        shop.OnShopAreaEntered += EnableShopUI;
        shop.OnShopAreaLeft += DisableShopUI;
        
        DisableShopUI();
        Initialize();
    }

    private void UpdateView()
    {
        foreach (var slot in _slots)
        {
            slot.UpdateVisuals();
        }
    }

    private void Initialize()
    {
        SpawnSlots();
        InitializeSlots();
    }

    private void SpawnSlots()
    {
        _slots = new();
        for (int i = 0; i < maxSlotsCount; i++)
        {
            ShopSlot spawned = Instantiate(slotPrefab, slotsParent);
            _slots.Add(spawned);
        }
    }

    private void InitializeSlots()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].Initialize(shop, i);
        }
    }
    
    private void EnableShopUI()
    {
        shopUIParent.SetActive(true);
    }

    private void DisableShopUI()
    {
        shopUIParent.SetActive(false);
    }
}
