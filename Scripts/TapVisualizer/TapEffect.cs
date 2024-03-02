using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class TapEffect : MonoBehaviour
{
    [SerializeField] private Transform spawnParent;
    [SerializeField] private Image effectImagePrefab;
    [SerializeField] private float animationTime;
    [SerializeField] private Vector2 scalePath;

    private void Start()
    {
        InputDetector.Instance.OnScreenTapped += SpawnClickEffectOnMousePosition;
    }

    private void SpawnClickEffectOnMousePosition()
    {
        Vector3 spawnPosition = Input.mousePosition;
        Image effectImage = Instantiate(effectImagePrefab, spawnPosition, quaternion.identity, spawnParent);
        AnimateEffect(effectImage);
    }

    private void AnimateEffect(Image image)
    {
        image.transform.localScale = scalePath.x * Vector3.one;
        image.transform.DOScale(Vector3.one * scalePath.y, animationTime);
        image.DOFade(0, animationTime).onComplete += () =>
        {
            Destroy(image.gameObject);
        };
    }
}
