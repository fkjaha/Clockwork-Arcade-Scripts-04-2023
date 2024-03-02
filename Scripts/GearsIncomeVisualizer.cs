using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GearsIncomeVisualizer : MonoBehaviour
{
    [SerializeField] private GearIncomeSubscriber gearIncomeSubscriber;
    [SerializeField] private TextMeshPool particlesPool;
    [SerializeField] private TextMeshProUGUI incomePerSecondText;
    [SerializeField] private Vector3 particleOffset;
    [SerializeField] private float yAnimation;
    [SerializeField] private float animationTime;
    [SerializeField] private GridMerge gridMerge;

    private void Start()
    {
        SubscribeAll();
    }

    private void SubscribeAll()
    {
        SubscribeRotationVisualization();   
        gearIncomeSubscriber.OnIncomePerSecondUpdated += UpdateIncomePerSecondText;
    }

    private protected virtual void SubscribeRotationVisualization()
    {
        foreach (Gear gear in FindObjectsOfType<Gear>())
        {
            if(gear is PowerGear) continue;
            SubscribeSingleGearVisualization(gear);
        }
    }

    private protected void SubscribeSingleGearVisualization(Gear gear, GearGridCell gridCell = null)
    {
        if (gridMerge != null)
        {
            gear.OnRotationCalled += () =>
            {
                VisualizeRotation(gear.transform.position,
                    gearIncomeSubscriber.GetIncome * (gridMerge.GetGearGridCellLevel(gridCell) + 1));
            };
            return;
        }
        
        gear.OnRotationCalled += () =>
        {
            VisualizeRotation(gear.transform.position, gearIncomeSubscriber.GetIncome);
        };
    }

    private void UpdateIncomePerSecondText()
    {
        incomePerSecondText.text = gearIncomeSubscriber.GetIncomePerSecond + "/sec";
    }

    private void VisualizeRotation(Vector3 position, int income)
    {
        TextMeshPro text = particlesPool.GetObject();
        text.text = "+" + income;
        AnimateText(position, text.transform);
    }
    
    private void AnimateText(Vector3 position, Transform target)
    {
        target.DOKill();
        target.position = position + particleOffset;
        target.gameObject.SetActive(true);
        target.DOMoveY(target.position.y + yAnimation, animationTime).onComplete += () => target.gameObject.SetActive(false);
    }
}
