using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float openTime;
    [SerializeField] private float openDegrees;
    [SerializeField] private Transform transformLeft;
    [SerializeField] private Transform transformRight;

    public void Open()
    {
        transformLeft.DOLocalRotate(Vector3.up * openDegrees, openTime);
        transformRight.DOLocalRotate(-Vector3.up * openDegrees, openTime);
    }
}
