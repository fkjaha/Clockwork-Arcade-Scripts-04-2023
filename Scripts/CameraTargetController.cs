using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CameraTargetController : MonoBehaviour
{
    [SerializeField] private Transform originalTarget;
    [SerializeField] private Transform target;

    public void Detach()
    {
        target.parent = null;
    }

    private void Attach()
    {
        target.parent = originalTarget;
    }

    public void LookAt(Vector3 position, float time)
    {
        target.DOMove(target.position.y * Vector3.up + position.Vector3ToFlat(), time);
    }
    
    public void LookAtAndReturn(Vector3 position, float time, UnityAction onCompleted = null)
    {
        Detach();
        target.DOMove(target.position.y * Vector3.up + position.Vector3ToFlat(), time).onComplete += () =>
        {
            Attach();
            target.DOLocalMove(target.localPosition.y * Vector3.up, time).onComplete += () => onCompleted?.Invoke();
        };
    }
}
