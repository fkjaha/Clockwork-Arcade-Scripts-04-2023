using System;
using System.Collections.Generic;
using UnityEngine;

public class GearGridSpin : MonoBehaviour
{
    public float GetRotationTime => gearRotationTime.GetSpeed;
    public float GetRotationLength => rotationLenght;
    
    [SerializeField] private GearGrid gearGrid;
    [SerializeField] private MainGear mainGear;
    [SerializeField] private GearRotationTime gearRotationTime;
    [SerializeField] private float rotationLenght;
    [SerializeField] private Vector2Int mainGearIndex;
    
    
    [SerializeField] private List<GearGridCell> _rightRotationCells = new();
    [SerializeField] private List<GearGridCell> _leftRotationCells = new();

    private void Awake()
    {
        mainGear.OnRotationCalled += () => RotateCandidates();
        gearGrid.OnGridCreated += () =>
        {
            CountNewRotationCandidates();
            // gearGrid.GetCells[mainGearIndex.y * gearGrid.GetGridSize.x + mainGearIndex.x].OnRotate += RotateCandidates;
        };
        gearGrid.OnGridUpdated += CountNewRotationCandidates;
        gearGrid.OnGridCellUpdated += cell =>
        {
            CountNewRotationCandidates();
            if(!cell.IsFilled()) return;
            float multiplier = 1 - mainGear.GetRotationProgress;
            if(Math.Abs(multiplier) < .0001f) return;
            int rotationDirectionMultiplier = _rightRotationCells.Contains(cell) ? 1 : (_leftRotationCells.Contains(cell) ? -1 : 0);
            if(rotationDirectionMultiplier == 0) return; 
            StopCandidates();
            RotateCandidates(multiplier);
            // cell.GetGear().RotateLenght(rotationDirectionMultiplier * rotationLenght * multiplier, gearRotationTime.GetSpeed * multiplier);
        };
        
    }

    public void CountNewRotationCandidates()
    {
        List<GearGridCell> cells = gearGrid.GetCells;
        _rightRotationCells = new();
        _leftRotationCells = new();
        
        Vector2Int gridSize = gearGrid.GetGridSize;
        bool[,] indexesToRotate = new bool[gridSize.x, gridSize.y];
        AddRotationCandidateIfPossible(mainGearIndex.x, mainGearIndex.y, gridSize.x, cells, ref indexesToRotate);

        int maxXIndex = gridSize.x - 1;
        int maxYIndex = gridSize.y - 1;

        for (int i = mainGearIndex.y; i < gridSize.y; i++)
        {
            for (int j = mainGearIndex.x; j < gridSize.x; j++)
            {
                if (indexesToRotate[j, i] == true)
                {
                    //1
                    if (j != maxXIndex)
                    {
                        AddRotationCandidateIfPossible(j + 1, i, gridSize.x, cells, ref indexesToRotate);
                    }
                    if (j != 0)
                    {
                        AddRotationCandidateIfPossible(j - 1, i, gridSize.x, cells, ref indexesToRotate);
                    }
                    if (i != maxYIndex)
                    {
                        AddRotationCandidateIfPossible(j, i + 1, gridSize.x, cells, ref indexesToRotate);
                    }
                    if (i != 0)
                    {
                        AddRotationCandidateIfPossible(j, i - 1, gridSize.x, cells, ref indexesToRotate);
                    }
                }
            }
        }
        
        for (int i = mainGearIndex.y; i > -1; i--)
        {
            for (int j = mainGearIndex.x; j > -1; j--)
            {
                if (indexesToRotate[j, i] == true)
                {
                    //1
                    if (j != maxXIndex)
                    {
                        AddRotationCandidateIfPossible(j + 1, i, gridSize.x, cells, ref indexesToRotate);
                    }
                    if (j != 0)
                    {
                        AddRotationCandidateIfPossible(j - 1, i, gridSize.x, cells, ref indexesToRotate);
                    }
                    if (i != maxYIndex)
                    {
                        AddRotationCandidateIfPossible(j, i + 1, gridSize.x, cells, ref indexesToRotate);
                    }
                    if (i != 0)
                    {
                        AddRotationCandidateIfPossible(j, i - 1, gridSize.x, cells, ref indexesToRotate);
                    }
                }
            }
        }
        
        Debug.Log("A");
    }

    private void RotateCandidates(float multiplier = 1)
    {
        foreach (GearGridCell cell in _rightRotationCells)
        {
            cell.GetGear().RotateLenght(rotationLenght * multiplier, gearRotationTime.GetSpeed);
        }
        foreach (GearGridCell cell in _leftRotationCells)
        {
            cell.GetGear().RotateLenght(-rotationLenght * multiplier,gearRotationTime.GetSpeed);
        }
    }
    
    private void StopCandidates()
    {
        foreach (GearGridCell cell in _rightRotationCells)
        {
            cell.GetGear().StopRotation();
        }
        foreach (GearGridCell cell in _leftRotationCells)
        {
            cell.GetGear().StopRotation();
        }
    }
    
    private void AddRotationCandidateIfPossible(int x, int y, int xSize, List<GearGridCell> cells, ref bool[,] rotationIndexes)
    {
        int tempListIndex = y * xSize + x;
        if (!rotationIndexes[x, y] && cells[tempListIndex].IsFilled())
        {
            rotationIndexes[x, y] = true;
            // Debug.Log(tempListIndex + " | " + tempListIndex%2);
            if((x + y) % 2 == 0)
                _rightRotationCells.Add(cells[tempListIndex]);
            else
                _leftRotationCells.Add(cells[tempListIndex]);
        }
    }
}
