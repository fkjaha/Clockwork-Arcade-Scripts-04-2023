using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Pool<T> : MonoBehaviour where T: UnityEngine.Object
{
    [SerializeField] private T instancePrefab;
    [SerializeField] private Transform poolParent;
    [SerializeField] private int poolSize;
    [SerializeField] private bool disableObjectsOnInit;

    private Queue<T> _pool;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _pool = new Queue<T>();
        for (int i = 0; i < poolSize; i++)
        {
            T spawned = Instantiate(instancePrefab, poolParent);
            _pool.Enqueue(spawned);
            if (disableObjectsOnInit)
            {
                spawned.GameObject().SetActive(false);
            }
        }
    }

    public T GetObject()
    {
        T result = _pool.Dequeue();
        _pool.Enqueue(result);
        return result;
    }
}
