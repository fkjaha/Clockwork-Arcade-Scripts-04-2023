using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GridMerge : MonoBehaviour
{
    [SerializeField] private KeyCode mergeInput;
    [SerializeField] private GearGrid gearGrid;
    [SerializeField] private int countToMerge;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material upgradedMaterial;
    [SerializeField] private Material upgradedMaterial2;
    [SerializeField] private float mergeTime;
    private readonly List<GearGridCell> _upgradedGears = new();
    private readonly List<GearGridCell> _upgradedGears2 = new();
    private GearGridCell _mergeCandidate;
    private List<GearGridCell> _candidates = new();
    private int _mergeableCount;

    private void Update()
    {
        if (Input.GetKeyDown(mergeInput))
        {
            TryMergeGears();
        }
    }

    public int GetGearGridCellLevel(GearGridCell gearGridCell)
    {
        if (_upgradedGears2.Contains(gearGridCell)) return 2;
        if (_upgradedGears.Contains(gearGridCell)) return 1;
        return 0;
    }

    public void TryMergeGears()
    {
        foreach (GearGridCell cell in gearGrid.GetCells)
        {
            if (!_upgradedGears.Contains(cell))
            {
                
                if(!cell.IsFilled()) continue;


                if (_mergeCandidate == null)
                {
                    _mergeCandidate = cell;
                }
                else
                {
                    _candidates.Add(cell);
                }

                if (_candidates.Count == (countToMerge - 1) && _mergeCandidate != null)
                {
                    MergeCandidate();
                    return;
                }
            }
        }
        
        _mergeCandidate = null;
        _candidates = new ();
        
        foreach (GearGridCell cell in gearGrid.GetCells)
        {
            if (!_upgradedGears2.Contains(cell) && _upgradedGears.Contains(cell)) 
            {
                
                if(!cell.IsFilled()) continue;


                if (_mergeCandidate == null)
                {
                    _mergeCandidate = cell;
                }
                else
                {
                    _candidates.Add(cell);
                }

                if (_candidates.Count == (countToMerge - 1) && _mergeCandidate != null)
                {
                    MergeCandidate2();
                    return;
                }
            }
        }
        
        _mergeCandidate = null;
        _candidates = new ();
    }

    private void MergeCandidate()
    {
        foreach (GearGridCell cell in _candidates)
        {
            Gear gear = cell.EmptyCell();
            Vector3 gearPos = gear.transform.position;
            gear.transform.DOMove(_mergeCandidate.transform.position, mergeTime).onComplete += () =>
            {
                gear.DisableGearFunctionality();
                gear.transform.position = gearPos;
            };
        }
        _upgradedGears.Add(_mergeCandidate);
        GearVisual gearVisual = _mergeCandidate.GetGear().GetComponent<GearVisual>();
        gearVisual.GetVisuals.GetComponent<MeshRenderer>().material = upgradedMaterial;
        gearVisual.GetParticles.SetActive(true);
        _mergeCandidate = null;
        _candidates = new ();
    }
    
    private void MergeCandidate2()
    {
        foreach (GearGridCell cell in _candidates)
        {
            Gear gear = cell.EmptyCell();
            Vector3 gearPos = gear.transform.position;
            gear.transform.DOMove(_mergeCandidate.transform.position, mergeTime).onComplete += () =>
            {
                _upgradedGears.Remove(cell);
                GearVisual gearVisual = gear.GetComponent<GearVisual>();
                gearVisual.GetVisuals.GetComponent<MeshRenderer>().material = defaultMaterial;
                gearVisual.GetParticles.SetActive(false);
                gear.DisableGearFunctionality();
                gear.transform.position = gearPos;
            };
        }
        _upgradedGears2.Add(_mergeCandidate);
        _mergeCandidate.GetGear().GetComponent<GearVisual>().GetVisuals.GetComponent<MeshRenderer>().material = upgradedMaterial2;
        _mergeCandidate = null;
        _candidates = new ();
    }
}
