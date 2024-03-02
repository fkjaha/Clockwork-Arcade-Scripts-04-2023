using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CoinsAnimator : MonoBehaviour
{
    public static CoinsAnimator Instance;
    
    [SerializeField] private Pool<Image> coinImagesPool;
    [SerializeField] private RectTransform targetImagesPositionTransform;
    [SerializeField] private float randomMoveDistance;
    [SerializeField] private float animationScaleDuration;
    [SerializeField] private float animationPositionDuration;
    [SerializeField] private Vector2 xAnimationBounds;
    [SerializeField] private Vector2 yAnimationBounds;

    private Vector2 _targetImagesPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        _targetImagesPosition = targetImagesPositionTransform.position;
    }

    public void AnimateSingleImage()
    {
        AnimateSingleImage(Vector2.zero);
    }
    
    public void AnimateSingleImage(Vector2 startPosition)
    {
        Image imageToAnimate = coinImagesPool.GetObject();
        // imageToAnimate.rectTransform.DOKill();
        imageToAnimate.rectTransform.DOComplete();
        imageToAnimate.rectTransform.DOKill();
        imageToAnimate.gameObject.SetActive(true);
        imageToAnimate.rectTransform.position = startPosition;
        imageToAnimate.rectTransform.DOMove(
            imageToAnimate.rectTransform.position + GetRandomDirection().normalized * randomMoveDistance, animationScaleDuration)
            .SetEase(Ease.InOutCirc);
        imageToAnimate.rectTransform.DOScale(Vector3.zero, animationScaleDuration).From().OnComplete(() =>
        {
            imageToAnimate.rectTransform.DOMove(targetImagesPositionTransform.position, animationScaleDuration).SetEase(Ease.InOutCirc).OnComplete(() => imageToAnimate.gameObject.SetActive(false));
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetImagesPositionTransform.position, 30f);
    }

    private Vector3 GetRandomDirection()
    {
        return new Vector2(Random.Range(xAnimationBounds.x, xAnimationBounds.y), Random.Range(yAnimationBounds.x, yAnimationBounds.y));
    }
}
