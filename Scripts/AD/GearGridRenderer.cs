using System;
using System.Collections.Generic;
using UnityEngine;

public class GearGridRenderer : MonoBehaviour
{
    [SerializeField] private GearGrid gridToRender;
    [SerializeField] private GearGridCell cellPrefab;
    private List<GearGridCell> cellRenderers = new();
    //
    // private void Awake()
    // {
    //     gridToRender.OnGridCreated += CreateRenderers;
    //     gridToRender.OnGridUpdated += UpdateGrid;
    //     gridToRender.OnGridCellUpdated += UpdateSingleCell;
    // }
    //
    // private void CreateRenderers()
    // {
    //     foreach (GearGridCell cell in gridToRender.GetCells)
    //     {
    //         CreateCellRenderer(cell);
    //     }
    // }
    //
    // private void CreateCellRenderer(GearGridCell cell)
    // {
    //     GearGridCell cell = Instantiate(cellPrefab, cell.GetPosition, Quaternion.identity);
    //     cellRenderers.Add(cell);
    // }
    //
    // private void UpdateGrid()
    // {
    //     
    // }
    //
    // private void UpdateSingleCell(GridCell gridCell)
    // {
    //     
    // }
}
