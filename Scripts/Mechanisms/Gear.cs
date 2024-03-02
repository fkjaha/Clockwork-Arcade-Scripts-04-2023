using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Gear : MonoBehaviour
{
    public event UnityAction OnEnabled;
    public event UnityAction OnRotationCalled;
    public event UnityAction OnRotationEnded;
    
    public float GetRadius => radius;
    public bool GetIsEnabled => _enabled;

    public float GetRotationProgress => _rotationProgress;
    private protected float GetLenght => _lenght;
    private protected bool IsActive => rotationTargetTransform.gameObject.activeSelf && _enabled;

    
    [SerializeField] private Transform rotationTargetTransform;
    [SerializeField] private float radius;

    private IEnumerator _rotationCoroutine;
    private float _rotationProgress = 1;
    private float _lenght;
    private bool _enabled;

    private protected virtual void Start()
    {
        _lenght = StaticFunctions.GetCircleLength(radius);
        UpdateVisibility();
    }

    public virtual void RotateLenght(float lenght, float time, UnityAction onCompleted = null)
    {
        if(!IsActive) return;
        
        float degrees = lenght / _lenght * 360;
        _rotationCoroutine = RotateOverTime(degrees, time, onCompleted);
        StartCoroutine(_rotationCoroutine);
        OnRotationCalled?.Invoke();
    }
    
    private IEnumerator RotateOverTime(float degrees, float time, UnityAction onCompleted = null)
    {
        float speed = degrees / time;
        float timePast = 0;
        while (timePast < time)
        {
            timePast += Time.deltaTime;
            _rotationProgress = timePast / time;
            rotationTargetTransform.rotation =
                    Quaternion.AngleAxis(speed * Time.deltaTime, rotationTargetTransform.up) * rotationTargetTransform.rotation;
            yield return new WaitForEndOfFrame();
        }
        OnRotationEnded?.Invoke();
        onCompleted?.Invoke();
    }

    public virtual void StopRotation()
    {
        StopAllCoroutines();
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(rotationTargetTransform.position, rotationTargetTransform.up, radius);
    }
    #endif

    public void EnableGearFunctionality()
    {
        _enabled = true;
        UpdateVisibility();
        OnEnabled?.Invoke();
    }
    
    public void DisableGearFunctionality()
    {
        _enabled = false;
        UpdateVisibility();
        // OnEnabled?.Invoke();
    }

    private void UpdateVisibility()
    {
        rotationTargetTransform.gameObject.SetActive(_enabled);
    }

    public void EnableVisuals()
    {
        rotationTargetTransform.gameObject.SetActive(true);
    }
}