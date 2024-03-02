using UnityEngine;

public class GearVisual : MonoBehaviour
{
    public Transform GetVisuals => visuals;
    public GameObject GetParticles => particles;
    
    [SerializeField] private Transform visuals;
    [SerializeField] private GameObject particles;
}
