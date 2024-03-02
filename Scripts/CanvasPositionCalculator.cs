using System;
using UnityEngine;

public class CanvasPositionCalculator : MonoBehaviour
{
    public static CanvasPositionCalculator Instance;
    
    [SerializeField] private Camera mainViewCamera;

    private void Awake()
    {
        Instance = this;
    }

    public Vector2 GetScreenPositionVector2(Vector3 worldPosition)
    {
        return GetScreenPositionVector3(worldPosition);
    }
    
    public Vector3 GetScreenPositionVector3(Vector3 worldPosition)
    {
        return mainViewCamera.WorldToScreenPoint(worldPosition);
    }
}
