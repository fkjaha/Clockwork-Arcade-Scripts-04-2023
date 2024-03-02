using UnityEngine;

public class GearGridIncomeVisualizer : GearsIncomeVisualizer
{
    [Header("Gear Grid Subscriber")]
    [SerializeField] private GearGrid gearGrid;

    private protected override void SubscribeRotationVisualization()
    {
        gearGrid.OnGridCellUpdated += cell =>
        {
            if (cell.IsFilled())
            {
                Gear gear = cell.GetGear();
                SubscribeSingleGearVisualization(gear, cell);
            }
        };
    }
}
