using System.Collections;
using System.Collections.Generic;

public interface IGameLoopHandler
{
    IEnumerator GameLoopRoutine { get; }
    List<TileImageController> FallDownCache { get; set; }
    List<TileImageController> MatchingImagesCache { get; }
}

