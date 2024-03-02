using DG.Tweening;
using UnityEngine;

public class GearGridPlacer : MonoBehaviour
{
    [SerializeField] private Gear gearPrefab;
    [SerializeField] private Camera raycastCamera;
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private KeyCode newGearKey;
    [SerializeField] private KeyCode placeKey;
    [SerializeField] private Vector3 currentGearPositionOffset;
    [SerializeField] private float releaseTime;

    private Gear _currentGear;
    private Transform _currentGearTransform;
    
    private void Update()
    {
        if (Input.GetKeyDown(newGearKey))
        {
            CreateNewCurrentGear();
        }
        
        if(_currentGear == null) return;

        Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, raycastMask)) return;
        
        if (Input.GetKeyDown(placeKey))
        {
            if(!hitInfo.collider.gameObject.TryGetComponent(out GearGridCell gridCell)) return;
            TrySetGear(gridCell);
        }
        else
        {
            _currentGearTransform.position = hitInfo.point + currentGearPositionOffset;
        }
    }

    private void TrySetGear(GearGridCell cell)
    {
        if(_currentGear == null) return;

        if (cell.TryFillCell(_currentGear))
        {
            _currentGearTransform.transform.DOLocalMove(cell.transform.position, releaseTime).onComplete += _currentGear.EnableGearFunctionality;
            ResetCurrentGear();
        }
    }

    private void ResetCurrentGear()
    {
        _currentGear = null;
    }

    public void CreateNewCurrentGear()
    {
        if(_currentGear != null) return; 
        
        SetCurrentGear(CreateNewGear());   
    }

    private Gear CreateNewGear()
    {
        return Instantiate(gearPrefab, Vector3.zero, Quaternion.identity);
    }

    private void SetCurrentGear(Gear gear)
    {
        _currentGear = gear;
        _currentGearTransform = _currentGear.transform;
        _currentGearTransform.transform.DOScale(Vector3.one* .001f, releaseTime).From().onComplete += _currentGear.EnableGearFunctionality;
        _currentGear.EnableGearFunctionality();
    }
}
