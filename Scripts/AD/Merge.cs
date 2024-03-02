using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Merge : MonoBehaviour
{
    [SerializeField] private KeyCode mergeInput;
    [SerializeField] private List<Gear> gears;
    [SerializeField] private int countToMerge;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material upgradedMaterial;
    [SerializeField] private Material upgradedMaterial2;
    [SerializeField] private float mergeTime;
    private readonly List<Gear> _upgradedGears = new();
    private readonly List<Gear> _upgradedGears2 = new();
    private Gear _mergeCandidate;
    private List<Gear> _candidates = new();
    private int _mergeableCount;

    private void Update()
    {
        if (Input.GetKeyDown(mergeInput))
        {
            TryMergeGears();
        }
    }

    public void TryMergeGears()
    {
        foreach (Gear gear in gears)
        {
            if (!_upgradedGears.Contains(gear))
            {
                
                if(!gear.GetIsEnabled) continue;


                if (_mergeCandidate == null)
                {
                    _mergeCandidate = gear;
                }
                else
                {
                    _candidates.Add(gear);
                }

                if (_candidates.Count == (countToMerge - 1) && _mergeCandidate != null)
                {
                    MergeCandidate();
                    return;
                }
            }
        }
        
        _mergeCandidate = null;
        _candidates = new List<Gear>();
        
        foreach (Gear gear in gears)
        {
            if (!_upgradedGears2.Contains(gear) && _upgradedGears.Contains(gear)) 
            {
                
                if(!gear.GetIsEnabled) continue;


                if (_mergeCandidate == null)
                {
                    _mergeCandidate = gear;
                }
                else
                {
                    _candidates.Add(gear);
                }

                if (_candidates.Count == (countToMerge - 1) && _mergeCandidate != null)
                {
                    MergeCandidate2();
                    return;
                }
            }
        }
        
        _mergeCandidate = null;
        _candidates = new List<Gear>();
    }

    private void MergeCandidate()
    {
        foreach (Gear gear in _candidates)
        {
            Vector3 gearPos = gear.transform.position;
            gear.transform.DOMove(_mergeCandidate.transform.position, mergeTime).onComplete += () =>
            {
                gear.DisableGearFunctionality();
                gear.transform.position = gearPos;
            };
        }
        _upgradedGears.Add(_mergeCandidate);
        GearVisual gearVisual = _mergeCandidate.GetComponent<GearVisual>();
        gearVisual.GetVisuals.GetComponent<MeshRenderer>().material = upgradedMaterial;
        gearVisual.GetParticles.SetActive(true);
        _mergeCandidate = null;
        _candidates = new List<Gear>();
    }
    
    private void MergeCandidate2()
    {
        foreach (Gear gear in _candidates)
        {
            Vector3 gearPos = gear.transform.position;
            gear.transform.DOMove(_mergeCandidate.transform.position, mergeTime).onComplete += () =>
            {
                _upgradedGears.Remove(gear);
                GearVisual gearVisual = gear.GetComponent<GearVisual>();
                gearVisual.GetVisuals.GetComponent<MeshRenderer>().material = defaultMaterial;
                gearVisual.GetParticles.SetActive(false);
                gear.DisableGearFunctionality();
                gear.transform.position = gearPos;
            };
        }
        _upgradedGears2.Add(_mergeCandidate);
        _mergeCandidate.GetComponent<GearVisual>().GetVisuals.GetComponent<MeshRenderer>().material = upgradedMaterial2;
        _mergeCandidate = null;
        _candidates = new List<Gear>();
    }
}
