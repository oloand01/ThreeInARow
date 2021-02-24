using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(CanvasGroup))]
public class GridController : MonoBehaviour, IGridController
{
    private ICellManager cellManager;
    private ITileManager tileManager;
    private CanvasGroup canvasGroup;
    private bool gridInteractable;

    public bool GridInteractable
    {
        get { return gridInteractable; }
        set
        {
            canvasGroup.interactable = value;
            gridInteractable = value;
        }
    }
    public IEnumerator CreateBoardRoutine { get; set; }
    public float TileFallSpeed { get; private set; } = 10f;

    [Inject]
    public void Init(ICellManager cellManager, ITileManager tileManager)
    {
        this.cellManager = cellManager;
        this.tileManager = tileManager;
    }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        GridInteractable = false;
        CreateBoardRoutine = CreateBoard();
    }

    public IEnumerator CreateBoard()
    {
        GridInteractable = false;
        yield return StartCoroutine(cellManager.CreateCellsRoutine);
        yield return StartCoroutine(tileManager.CreateTilesRoutine);
        CreateBoardRoutine = CreateBoard();
        GridInteractable = true;
    }

    public void ResizeItemsToFitScreen(GridLayoutGroup gridLayout, RectTransform gridContainer)
    {
        var layoutConstraintCount = (decimal)gridLayout.constraintCount;
        var rowsToColumnsRatio = gridContainer.childCount / layoutConstraintCount;
        var gridRows = (float)Math.Floor(rowsToColumnsRatio);

        if ((rowsToColumnsRatio % 1) != 0)
        {
            gridRows += 1;
        }

        if ((float)layoutConstraintCount >= gridRows * Screen.width / Screen.height)
        {
            gridLayout.cellSize = new Vector2(gridContainer.rect.width / (float)layoutConstraintCount, gridContainer.rect.width / (float)layoutConstraintCount);
        }
        else
        {
            gridLayout.cellSize = new Vector2(gridContainer.rect.height / gridRows, gridContainer.rect.height / gridRows);
        }
    }

    public void ResetBoard()
    {
        cellManager.PoolAllCells();
        tileManager.PoolAllTiles();
        StartCoroutine(CreateBoardRoutine);
    }
}
