using UnityEngine;

public class GearGridIncomeSubscriber : GearIncomeSubscriber
{
    [Header("Gear Grid Subscriber")]
    [SerializeField] private GearGrid gearGrid;
    
    private protected override void SubscribeAllGearsIncome()
    {
        gearGrid.OnGridCellUpdated += cell =>
        {
            if (cell.IsFilled())
            {
                Gear gear = cell.GetGear();
                SubscribeSingleGearIncome(gear);
            }
        };
    }
}
