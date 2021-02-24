using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;


public class TileMovementHandler : ITileMovementHandler
{
    private IGameLoopHandler gameLoopHandler;
    private IGridController gridController;

    public IEnumerator MoveTileImagesDownRoutine { get; private set; }

    [Inject]
    public void Init(IGameLoopHandler gameLoopHandler, IGridController gridController)
    {
        this.gameLoopHandler = gameLoopHandler;
        this.gridController = gridController;
        MoveTileImagesDownRoutine = MoveTileImagesDown();
    }

    private IEnumerator MoveTileImagesDown()
    {
        var tilesToFallDown = gameLoopHandler.FallDownCache;
        if (!tilesToFallDown.Any())
        {
            MoveTileImagesDownRoutine = MoveTileImagesDown();
            yield break;
        }

        float tileFallDownStep = gridController.TileFallSpeed * Time.deltaTime;
        while (TileImagesHaveDistanceToMove(tilesToFallDown))
        {
            for (int i = 0; i < tilesToFallDown.Count; i++)
            {
                tilesToFallDown[i].transform.position = Vector2.MoveTowards(tilesToFallDown[i].transform.position,
                    tilesToFallDown[i].targetPosition, tileFallDownStep);
            }

            yield return null;
        }

        for (int i = 0; i < tilesToFallDown.Count; i++)
        {
            tilesToFallDown[i].transform.position = tilesToFallDown[i].targetPosition;
        }
        tilesToFallDown.Clear();
        MoveTileImagesDownRoutine = MoveTileImagesDown();
        yield return new WaitForSeconds(0.02f);
    }

    private bool TileImagesHaveDistanceToMove(List<TileImageController> tilesToFallDown)
    {
        bool tilesHaveDistanceToMove = false;
        for (int i = 0; i < tilesToFallDown.Count; i++)
        {
            if (Vector2.Distance(tilesToFallDown[i].transform.position, tilesToFallDown[i].targetPosition) > 0.001f)
            {
                tilesHaveDistanceToMove = true;
                break;
            }
        }
        return tilesHaveDistanceToMove;
    }
}
