using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EndCutScene : MonoBehaviour
{
    public static EndCutScene Instance;
    
    public event UnityAction OnPlaybackStarted;

    [SerializeField] private PoliceSquad policeSquad;
    [SerializeField] private CameraTargetController cameraTargetController;
    [SerializeField] private Door door;
    [SerializeField] private ParticleSystem particleSystem;

    [SerializeField] private float cameraMoveTime;
    [SerializeField] private float lookAtOpenDoorTime;
    [SerializeField] private Transform doorPosition;
    [SerializeField] private Transform playerPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameStateManager.Instance.OnGameFinished += Play;
    }

    [ContextMenu("Play")]
    private void Play()
    {
        StartCoroutine(CutScene());
        OnPlaybackStarted?.Invoke();
    }

    private IEnumerator CutScene()
    {
        cameraTargetController.Detach();
        
        //camera at door
        cameraTargetController.LookAt(doorPosition.position, cameraMoveTime);
        yield return new WaitForSeconds(cameraMoveTime);
        // door open
        door.Open();
        // police come in
        policeSquad.ActivatePolicemans();
        yield return new WaitForSeconds(lookAtOpenDoorTime);
        // camera at doctor
        cameraTargetController.LookAt(playerPosition.position, cameraMoveTime);
        yield return new WaitForSeconds(cameraMoveTime);
        // angry emoji
        particleSystem.Play();
        
        Debug.Log("Cutscene PLAYED!");
    }
}
