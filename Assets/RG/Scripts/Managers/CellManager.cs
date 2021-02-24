using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(GridLayoutGroup))]
public class CellManager : MonoBehaviour,ICellManager
{
    private ILevelManager levelManager;
    private IObjectPoolManager objectPoolManager;
    private RectTransform cellContainer;
    private GridLayoutGroup cellLayout;
    private List<GameObject> cells;

    public IEnumerator CreateCellsRoutine { get; set; }
    public IGridController gridController;

    [Inject]
    public void Init(ILevelManager levelManager, IObjectPoolManager objectPoolManager,IGridController gridController)
    {
        this.levelManager = levelManager;
        this.objectPoolManager = objectPoolManager;
        this.gridController = gridController;
    }

    void Awake()
    {
        cellLayout = GetComponent<GridLayoutGroup>();
        cellContainer = GetComponent<RectTransform>();
        cells = new List<GameObject>();
        CreateCellsRoutine = CreateCells();

        Assert.IsTrue(cellLayout.startCorner == GridLayoutGroup.Corner.LowerLeft, "CellGrid has so be lower left");
        Assert.IsTrue(cellLayout.startAxis == GridLayoutGroup.Axis.Horizontal, "CellGrid has so be Horizontal");
        Assert.IsTrue(cellLayout.constraint == GridLayoutGroup.Constraint.FixedColumnCount, "CellGrid has so be fixedColumn");
        Assert.IsTrue(cellLayout.childAlignment == TextAnchor.MiddleCenter, "CellGrid has so be Middle center");
        Assert.IsTrue(cellLayout.spacing == Vector2.zero, "CellGrid has so be 0 spacing");
    }

    private void OnRectTransformDimensionsChange()
    {
        if (cellLayout != null)
            gridController.ResizeItemsToFitScreen(cellLayout, cellContainer);
    }

    private IEnumerator CreateCells()
    {
        cellLayout.constraintCount = levelManager.CurrentLevel.BoardGrid.Columns;
        decimal Cells = levelManager.CurrentLevel.BoardGrid.Columns * levelManager.CurrentLevel.BoardGrid.Rows;
  
        for (int i = 0; i < Cells; i++)
            CreateCell(levelManager.CurrentLevel);

        gridController.ResizeItemsToFitScreen(cellLayout, cellContainer);
        CreateCellsRoutine = CreateCells();
        yield return null;
    }

    private void CreateCell(Level currentLevel)
    {
        var cell = objectPoolManager.GetPooledCell(currentLevel.BoardGrid.cell.gameObject.name);
        cell.transform.SetParent(cellContainer, false);
        cell.transform.localPosition = Vector3.zero;
        cell.SetActive(true);
        cells.Add(cell);
    }

    public void PoolAllCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            objectPoolManager.PoolCell(cells[i]);
        }
    }
}

