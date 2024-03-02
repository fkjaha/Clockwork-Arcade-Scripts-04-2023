using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MouseCogPlacer : MonoBehaviour
{
    [SerializeField] private KeyCode setKeyCode;
    [SerializeField] private KeyCode releaseKeyCode;
    [SerializeField] private Camera raycastCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 offset;
    [SerializeField] private List<GearVisual> gears;
    [SerializeField] private float releaseTime;
    [SerializeField] private float scaleTime;

    private Gear _activeGear;
    private Transform _activeGearTransform;
    private int _activeGearIndex;

    public void SetActiveGear()
    {
        foreach (GearVisual gear in gears)
        {
            _activeGear = gear.GetComponent<Gear>();
            if (!_activeGear.GetIsEnabled)
            {
                _activeGearTransform = gear.GetVisuals;
        
        
                _activeGearTransform.localScale = Vector3.one * .01f;
                _activeGear.EnableVisuals();
                _activeGearTransform.DOScale(Vector3.one, scaleTime);
                
                break;
            } 
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(setKeyCode))
        {
            SetActiveGear();
        }
        if (Input.GetKeyDown(releaseKeyCode))
        {
            ReleaseActive();
        }
        
        
        
        if(_activeGearTransform == null) return;
        
        Ray raycast = raycastCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(raycast, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            _activeGearTransform.position = hit.point + offset;
        }
    }

    private void ReleaseActive()
    {
        _activeGearTransform.DOLocalMove(Vector3.zero, releaseTime).onComplete += _activeGear.EnableGearFunctionality;
        _activeGearTransform = null;
    }
}
