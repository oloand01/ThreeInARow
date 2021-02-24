using System.Collections.Generic;

public interface IMatchMaker
{
    List<TileImageController> GetMatchingTileImages(List<TileController> tiles, int constraintCount, int requiredMatchesInARow);
}
