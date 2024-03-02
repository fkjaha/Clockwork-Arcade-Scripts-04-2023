using UnityEngine;

public class FramerateSetter : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }
}
