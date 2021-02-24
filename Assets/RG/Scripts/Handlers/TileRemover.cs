using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

public class TileRemover : ITileRemover
{
    private IGameLoopHandler gameLoopHandler;
    public IEnumerator FadeoutTilesWhenMatchedRoutine { get; set; }

    [Inject]
    public void Init(IGameLoopHandler gameLoopHandler)
    {
        this.gameLoopHandler = gameLoopHandler;
        this.FadeoutTilesWhenMatchedRoutine = FadeoutTilesWhenMatched();
    }

    private IEnumerator FadeoutTilesWhenMatched()
    {
        if (!gameLoopHandler.MatchingImagesCache.Any())
        {
            FadeoutTilesWhenMatchedRoutine = FadeoutTilesWhenMatched();
            yield break;
        }

        float tileImageAlpha = 1;
        while (tileImageAlpha > 0.15f)
        {
            tileImageAlpha = Mathf.MoveTowards(tileImageAlpha, 0, 1 * Time.deltaTime);

            for (int i = 0; i < gameLoopHandler.MatchingImagesCache.Count; i++)
            {
                gameLoopHandler.MatchingImagesCache[i].donutImage.color = new Color(255, 255, 255, tileImageAlpha);
            }

            yield return null;
        }
        FadeoutTilesWhenMatchedRoutine = FadeoutTilesWhenMatched();
    }
}

