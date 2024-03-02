using DG.Tweening;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int GetWorth => worth;
    public Transform GetVisualsTransform => visualsTransform;
    
    [SerializeField] private int worth;
    [SerializeField] private Transform visualsTransform;

    public void SetWorth(int newWorth)
    {
        worth = newWorth;
    }
    
    private void OnDestroy()
    {
        visualsTransform.DOKill();
    }
}
