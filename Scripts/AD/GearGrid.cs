using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GearGrid : MonoBehaviour
{
    public event UnityAction OnGridCreated;
    public event UnityAction OnGridUpdated;
    public event UnityAction<GearGridCell> OnGridCellUpdated;
    
    public Vector2Int GetGridSize => gridSize;
    public Vector2 GetGridPadding => gridPadding;
    public List<GearGridCell> GetCells => cells;

    [Header("Metrics")]
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private Vector2 gridPadding;
    [SerializeField] private float yOffset;
    
    [Header("Other")]
    [SerializeField] private Transform gridOrigin;
    [SerializeField] private Transform cellsParent;
    [SerializeField] private GearGridCell cellPrefab;
    [SerializeField] private List<GearGridCell> cells;

    private void Start()
    {
        CreateGrid();
    }

    [ContextMenu("CreateGrid")]
    public void CreateGrid()
    {
        ClearGrid();
        SpawnCells();
        OnGridCreated?.Invoke();
        OnGridUpdated?.Invoke();
    }

    private void SpawnCells()
    {
        Vector3 originPosition = gridOrigin.position;
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                SpawnSingleCell(originPosition + new Vector3(i * gridPadding.x, yOffset, j * gridPadding.y));
            }
        }
    }

    private void SpawnSingleCell(Vector3 position)
    {
        GearGridCell cell = Instantiate(cellPrefab, position, Quaternion.identity, cellsParent);
        cells.Add(cell);
        cell.OnUpdated += () => OnGridCellUpdated?.Invoke(cell);
    }
    
    private void ClearGrid()
    {
        cells = new();
    }
}
