using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private List<GameObject> tutorialObjects;
    [SerializeField] private CameraTargetController cameraTargetController;
    [SerializeField] private float cameraMoveTime;
    [SerializeField] private List<TutorialStage> stages;
    [SerializeField] private Transform arrowTransform;
    [SerializeField] private int startIndex;
    [SerializeField] private int finalIndex;

    [SerializeField] private Gear firstBuyAreaTransform;
    [SerializeField] private Gear secondBuyAreaTransform;
    [SerializeField] private Gear lastBuyAreaTransform;
    [SerializeField] private Shop shop;
    [SerializeField] private PlayerArea moneyArea;

    private int _shopTapped;
    private int _tapped;


    private void EndTutorial()
    {
        foreach (GameObject tutorialObject in tutorialObjects)
        {
            Destroy(tutorialObject);
        }
        Destroy(this);
    }

    private void Start()
    {
        if(finalIndex == 0) EndTutorial();
        else
            GoToStage(startIndex);
    }

    public void GoToStage(int index)
    {
        TutorialStage stage = stages[index];

        arrowTransform.position = stage.GetArrowPosition;
        foreach (GameObject getActiveBound in stage.GetActiveBounds)
        {
            getActiveBound.SetActive(true);
        }
        foreach (GameObject getPassiveBound in stage.GetPassiveBounds)
        {
            getPassiveBound.SetActive(false);
        }
        if(stage.GetStartPlayer != null)
            stage.GetStartPlayer.PlayFeedbacks();
        if(index > 0 && stages[index-1].GetFinishPlayer != null)
            stages[index-1].GetFinishPlayer.PlayFeedbacks();

        AddListeners(index);
        if (stage.GetMoveCamera)
        {
            cameraTargetController.LookAtAndReturn(stage.GetCameraPosition, cameraMoveTime, () =>
            {
                if(finalIndex == index) EndTutorial();
            });
        }
        else
        {
            if(finalIndex == index) EndTutorial();
        }
    }

    private void AddListeners(int index)
    {
        switch (index)
        {
            case 0:
                firstBuyAreaTransform.OnEnabled += GoTo1;
                break;
            case 1:
                firstBuyAreaTransform.OnEnabled -= GoTo1;
                secondBuyAreaTransform.OnEnabled += GoTo2;
                break;
            case 2:
                secondBuyAreaTransform.OnEnabled -= GoTo2;
                shop.OnOptionPurchased += GoTo3;
                break;
            case 3:
                shop.OnOptionPurchased -= GoTo3;
                InputDetector.Instance.OnScreenTapped += GoTo4;
                break;
            case 4:
                InputDetector.Instance.OnScreenTapped -= GoTo4;
                moneyArea.OnPlayerEntered += GoTo5;
                break;
            case 5:
                moneyArea.OnPlayerEntered -= GoTo5;
                lastBuyAreaTransform.OnEnabled += GoTo6;
                break;
            case 6:
                lastBuyAreaTransform.OnEnabled -= GoTo6;
                break;
        }
    }

    private void GoTo4()
    {
        _tapped++;
        if(_tapped == 5)
            GoToStage(4);
    }
    
    private void GoTo5()
    {
        GoToStage(5);
    }
    private void GoTo6()
    {
        GoToStage(6);
    }
    
    private void GoTo1()
    {
        GoToStage(1);
    }
    
    private void GoTo2()
    {
        GoToStage(2);
    }
    
    private void GoTo3()
    {
        _shopTapped++;
        if(_shopTapped == 2)
            GoToStage(3);
    }
}

[Serializable]
public class TutorialStage
{
    public List<GameObject> GetActiveBounds => activeBounds;
    public List<GameObject> GetPassiveBounds => passiveBounds;
    public Vector3 GetArrowPosition => arrowTarget.position.Vector3ToFlat();
    public Vector3 GetCameraPosition => cameraTarget.position.Vector3ToFlat();
    public bool GetMoveCamera => moveCamera;
    public MMF_Player GetStartPlayer => startPlayer;
    public MMF_Player GetFinishPlayer => finishPlayer;

    [SerializeField] private List<GameObject> activeBounds;
    [SerializeField] private List<GameObject> passiveBounds;
    [SerializeField] private Transform arrowTarget;
    [SerializeField] private bool moveCamera;
    [SerializeField] private Transform cameraTarget;

    [SerializeField] private MMF_Player startPlayer;
    [SerializeField] private MMF_Player finishPlayer;
}
