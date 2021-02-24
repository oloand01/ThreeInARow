using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

public class GameLoopHandler : MonoBehaviour, IGameLoopHandler
{
    private ITileRemover tileRemover;
    private IMatchMaker matchMaker;
    private ITileMovementHandler tileMovementHandler;
    private ITileManager tileManager;
    private ILevelManager levelManager;
    private IGridController gridController;
    public List<TileImageController> FallDownCache { get; set; }
    public List<TileImageController> MatchingImagesCache { get; set; }
    public IEnumerator GameLoopRoutine { get; private set; }

    [Inject]
    public void Init(ITileRemover tileRemover, 
        IMatchMaker matchMaker, 
        ITileMovementHandler tileMovementHandler,
        ITileManager tileManager, 
        ILevelManager levelManager,
        IGridController gridController)
    {
        this.tileRemover = tileRemover;
        this.matchMaker = matchMaker;
        this.tileMovementHandler = tileMovementHandler;
        this.tileManager = tileManager;
        this.levelManager = levelManager;
        this.gridController = gridController;

        MatchingImagesCache = new List<TileImageController>();
        FallDownCache = new List<TileImageController>();
        GameLoopRoutine = GameLoop();
    }

    public IEnumerator GameLoop()
    {
        Assert.IsTrue(tileManager.tiles.Count > 0,"Tiles is empty when gameloop is running");
        do
        {
            MatchingImagesCache.Clear();
            FallDownCache = tileManager.ReparentTileImages(FallDownCache);
            yield return StartCoroutine(tileMovementHandler.MoveTileImagesDownRoutine);
            MatchingImagesCache = matchMaker.GetMatchingTileImages(tileManager.tiles, tileManager.ConstraintCount, 
                levelManager.CurrentLevel.RequiredMatchesInARow);
            yield return StartCoroutine(tileRemover.FadeoutTilesWhenMatchedRoutine);
            tileManager.PoolMatchedTileImages(MatchingImagesCache);

            Assert.IsTrue(tileManager.tiles.Where(tile=>tile.childImage==null).Any(), "At least one tile should have empty child when loop has run");
       
        } while (MatchingImagesCache.Any());

        GameLoopRoutine = GameLoop();
        gridController.GridInteractable = true;
    }
}

