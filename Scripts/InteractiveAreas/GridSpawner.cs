using System;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private Transform originPosition;
    [SerializeField] private Vector2Int gridBaseSize;
    [SerializeField] private Vector3 spawnPadding;
    [SerializeField] private float gizmosSize;

    public T SpawnWithGrid<T>(T prefab, int index) where T: UnityEngine.Object
    {
        return Instantiate(prefab, GetGridPosition(index), Quaternion.identity);
    }

    public Vector3 GetGridPosition(int index)
    {
        return originPosition.position + new Vector3( 
            spawnPadding.x * (index% gridBaseSize.x),
            spawnPadding.y * (index/ (gridBaseSize.x * gridBaseSize.y)),
            spawnPadding.z * ((index/ gridBaseSize.x)%gridBaseSize.y));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < gridBaseSize.x; i++)
        {
            for (int j = 0; j < gridBaseSize.y; j++)
            {
                Gizmos.DrawSphere(originPosition.position 
                                  + new Vector3(i * spawnPadding.x, 0f, j * spawnPadding.z), gizmosSize);
            }
        }
    }
}
