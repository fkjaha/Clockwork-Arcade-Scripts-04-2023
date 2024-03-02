using UnityEngine;

public class UpgradeEffect : MonoBehaviour
{
    public static UpgradeEffect Instance;

    [SerializeField] private Pool<ParticleSystem> pool;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayUpgradeEffect(UpgradeArea upgradeArea)
    {
        ParticleSystem playSystem = pool.GetObject();
        playSystem.transform.position = upgradeArea.transform.position + offset;
        playSystem.Play();
    }
    
    public void PlayUpgradeEffect(BuyArea upgradeArea)
    {
        ParticleSystem playSystem = pool.GetObject();
        playSystem.transform.position = upgradeArea.transform.position + offset;
        playSystem.Play();
    }
}
