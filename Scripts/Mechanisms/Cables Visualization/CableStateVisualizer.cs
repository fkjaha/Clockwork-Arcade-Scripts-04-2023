using UnityEngine;

public abstract class CableStateVisualizer : MonoBehaviour
{
    [Header("State Visualizer")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material passiveMaterial;

    private Material[] _activeMaterials;
    private Material[] _passiveMaterials;

    private void Start()
    {
        _activeMaterials = new[] {activeMaterial, activeMaterial, activeMaterial};
        _passiveMaterials = new[] {passiveMaterial, passiveMaterial, passiveMaterial};
        
        SubscribeToEnableEvent();
        ApplyRightMaterial();
    }

    private protected abstract void SubscribeToEnableEvent();
    private protected abstract bool ConnectedStuffEnabled();

    private protected void ApplyRightMaterial()
    {
        meshRenderer.materials = ConnectedStuffEnabled() ? _activeMaterials : _passiveMaterials;
    }
}
