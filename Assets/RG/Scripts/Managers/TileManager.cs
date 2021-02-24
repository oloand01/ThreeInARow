using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(GridLayoutGroup))]
public class TileManager : MonoBehaviour, ITileManager
{
    private ILevelManager levelManager;
    private IObjectPoolManager objectPoolManager;
    private IGridController gridController;
    private RectTransform tileContainer;
    private GridLayoutGroup tileLayout;
    public List<TileController> tiles { get; set; }
    public IEnumerator CreateTilesRoutine { get; private set; }
    public int ConstraintCount { get { return tileLayout.constraintCount; } }

    [Inject]
    public void Init(ILevelManager levelManager, IObjectPoolManager objectPoolManager, IGridController gridController)
    {
        this.levelManager = levelManager;
        this.objectPoolManager = objectPoolManager;
        this.gridController = gridController;
    }

    void Awake()
    {
        tileLayout = GetComponent<GridLayoutGroup>();
        tileContainer = GetComponent<RectTransform>();
        tiles = new List<TileController>();
        CreateTilesRoutine = CreateTiles();
        Assert.IsTrue(tileLayout.startCorner == GridLayoutGroup.Corner.LowerLeft,"TileGrid has so be lower left");
        Assert.IsTrue(tileLayout.startAxis == GridLayoutGroup.Axis.Horizontal, "TileGrid has so be Horizontal");
        Assert.IsTrue(tileLayout.constraint == GridLayoutGroup.Constraint.FixedColumnCount, "TileGrid has so be fixedColumn");
        Assert.IsTrue(tileLayout.childAlignment == TextAnchor.MiddleCenter, "TileGrid has so be Middle center");
        Assert.IsTrue(tileLayout.spacing == Vector2.zero, "TileGrid has so be 0 spacing");
    }

    private void OnRectTransformDimensionsChange()
    {
        if (tileLayout != null)
            gridController.ResizeItemsToFitScreen(tileLayout, tileContainer);
    }

    private IEnumerator CreateTiles()
    {
        tileLayout.constraintCount = levelManager.CurrentLevel.BoardGrid.Columns;
        decimal tiles = levelManager.CurrentLevel.BoardGrid.Columns * levelManager.CurrentLevel.BoardGrid.Rows;
        int tileType = 0;
        for (int i = 0; i < tiles; i++)
        {
            if (tileType == levelManager.CurrentLevel.Tiles.Length)
                tileType = 0;

            var tile = CreateTile();

            CreateTileImage(levelManager.CurrentLevel, tileType, tile.GetComponent<TileController>());
            tileType++;
            gridController.ResizeItemsToFitScreen(tileLayout, tileContainer);
        }

        SetTileControllerProperties();
        CreateTilesRoutine = CreateTiles();
        yield return null;
    }

    private GameObject CreateTileImage(Level currentLevel, int tileType, TileController tileController)
    {
        var tileImage = objectPoolManager.GetPooledTileImage(currentLevel.Tiles[tileType].gameObject.name);
        tileController.childImage = tileImage.GetComponent<TileImageController>();
        tileImage.transform.SetParent(tileController.tileControllerRect, false);
        tileImage.SetActive(true);
        tiles.Add(tileController);
        return tileImage;
    }
    private GameObject CreateTile()
    {
        var tile = objectPoolManager.GetPooledTile();
        tile.transform.SetParent(tileContainer, false);
        tile.SetActive(true);
        return tile;
    }

    private void SetTileControllerProperties()
    {
        int tilesIndex = 0;
        for (int y = 0; y < tiles.Count / tileLayout.constraintCount; y++)
        {
            for (int x = 0; x < tileLayout.constraintCount; x++)
            {
                var tile = tiles[tilesIndex];
                tile.x = x;
                tile.y = y;
                tilesIndex++;
            }
        }
    }
    public List<TileImageController> ReparentTileImages(List<TileImageController> tileImageControllers)
    {
        for (int y = 0; y < tiles.Count / ConstraintCount; y++)
        {
            for (int x = 0; x < ConstraintCount; x++)
            {
                var tile = GetTileAtGridPosition(x, y);

                if (tile.childImage != null)
                {
                    continue;
                }

                var tileToReParent = GetFirstTileAbove(x, y);

                if (tileToReParent != null)
                {
                    CreateNewParentForImage(tile, tileToReParent);
                    tile.childImage.targetPosition = tile.transform.position;
                    tileImageControllers.Add(tile.childImage);
                }
            }
        }
        return tileImageControllers;
    }

    private TileController GetFirstTileAbove(int moveToPositionOnX, int moveToPositionOnY)
    {

        for (int y = 0; y < tiles.Count / ConstraintCount; y++)
        {
            for (int x = 0; x < ConstraintCount; x++)
            {
                var tempTile = GetTileAtGridPosition(x, y);

                if (x == moveToPositionOnX && y > moveToPositionOnY && tempTile.childImage != null)
                {
                    return tempTile;
                }
            }
        }
        return null;
    }

    private void CreateNewParentForImage(TileController newParent, TileController oldParent)
    {
        newParent.childImage = oldParent.childImage;
        oldParent.childImage.transform.SetParent(newParent.transform, true);
        oldParent.childImage = null;
    }

    public TileController GetTileAtGridPosition(int x, int y)
    {
        return tiles.Find(item => item.x == x && item.y == y);
    }
    public void PoolAllTiles() => objectPoolManager.PoolTileCollection(tiles);

    public void PoolMatchedTileImages(List<TileImageController> matchingImagesCache)
    {
        for (int i = 0; i < matchingImagesCache.Count; i++)
        {
            objectPoolManager.PoolTileImageController(matchingImagesCache[i]);
        }
    }
}

