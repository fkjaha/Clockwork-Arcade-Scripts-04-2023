using UnityEngine;

public class GridMachineConnector : MonoBehaviour
{
    [SerializeField] private GearGridSpin gearGridSpin;
    [SerializeField] private Vector2Int cellLocation;
    [SerializeField] private GearGrid grid;
    [SerializeField] private PowerGear machinePowerGear;
    [SerializeField] private GearGridCell cell;

    private void Awake()
    {
        grid.OnGridCreated += () =>
        {
            cell = grid.GetCells[cellLocation.y * grid.GetGridSize.x + cellLocation.x];
            cell.OnRotate += () =>
                machinePowerGear.RotateLenght(gearGridSpin.GetRotationLength, gearGridSpin.GetRotationTime);
            cell.OnUpdated += () =>
            {
                if(cell.IsFilled()) return;
                machinePowerGear.StopRotation();
            };
        };
    }
}
