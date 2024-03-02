using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnergyMachineVisualizer : MonoBehaviour
{
    [SerializeField] private MoneyMachine moneyMachine;
    [SerializeField] private Transform visualizeTransform;
    [SerializeField] private MainGear mainGear;
    [SerializeField] private GearIncomeSubscriber gearIncomeSubscriber;
    [SerializeField] private TextMeshPool particlesPool;
    [SerializeField] private Vector3 particleOffset;
    [SerializeField] private float yAnimation;
    [SerializeField] private float animationTime;

    private void Start()
    {
        // mainGear.OnRotationCalled += () =>
        // {
        //     VisualizeRotation(visualizeTransform.position, (int)gearIncomeSubscriber.GetIncomePerSecond);
        // };
        StartCoroutine(VisualizationRepeater());
    }

    private IEnumerator VisualizationRepeater()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if(moneyMachine.FirstUseRegistered)
                VisualizeRotation(visualizeTransform.position, (int)gearIncomeSubscriber.GetIncomePerSecond);
        }
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
