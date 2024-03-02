using TMPro;
using UnityEngine;

public class MoneyMachineInfoVisualizer : MonoBehaviour
{
    [SerializeField] private MoneySpawner moneySpawner;
    [SerializeField] private MoneyMachine moneyMachine;
    [SerializeField] private GearRotationTime mainGearRotationTime;
    [SerializeField] private TextMeshProUGUI infoText;

    private void Start()
    {
        moneySpawner.OnIncomePerSecondUpdated += UpdateInfo;
    }

    private void UpdateInfo()
    {
        float income = (int) (moneyMachine.GetMoneyWorth * moneyMachine.GetStacksPerRotation /
            mainGearRotationTime.GetSpeed * 10) / (float) 10;
        infoText.text = (moneyMachine.FirstUseRegistered ? income : 0)  + "/sec";
    }
}
