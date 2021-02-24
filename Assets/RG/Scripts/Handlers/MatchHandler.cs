using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class MatchHandler : IMatchMaker, IInitializable
{
    private List<TileController> temporaryMatches;
    private List<TileImageController> matchingImages;

    public void Initialize()
    {
        temporaryMatches = new List<TileController>();
        matchingImages = new List<TileImageController>();
    }

    public List<TileImageController> GetMatchingTileImages(List<TileController> tiles,int cellCount, int requiredMatchesInARow)
    {
        temporaryMatches.Clear();
        matchingImages.Clear();

        string lastTileName = string.Empty;
        int matchesInARow = 0;

        for (int row = 0; row < tiles.Count / cellCount; row++)
        {
            for (int column = 0; column < cellCount; column++)
            {
                var tile = tiles.Find(item => item.x == column && item.y == row);
                string tileName = GetTileName(tile);

                ResetMatchesIfWeAreOnNewRow(ref lastTileName, ref matchesInARow, column);

                if (lastTileName == tileName && lastTileName != string.Empty)
                {
                    matchesInARow++;
                    AddToMatchesBasedOnCount(matchesInARow, tile, tiles, requiredMatchesInARow);
                }
                else
                {
                    matchesInARow = 1;
                    lastTileName = tileName;
                }
            }
        }
        return matchingImages;
    }

    private void AddToMatchesBasedOnCount(int matchesInARow, TileController tile, List<TileController> tiles, int requiredMatchesInARow)
    {
        if (matchesInARow == requiredMatchesInARow)
        {
            for (int i = 0; i < requiredMatchesInARow; i++)
            {
                var tileController=tiles.Find(item => item.x == (tile.x - i) && item.y == tile.y);
                MatchAndPool(tileController);
            }
        }
        else if (matchesInARow > requiredMatchesInARow)
        {
            MatchAndPool(tile);
        }
    }

    private void MatchAndPool(TileController tile)
    {
        temporaryMatches.Add(tile);
        matchingImages.Add(tile.childImage);
        tile.childImage = null;
    }

    private string GetTileName(TileController tile)
    {
        if (tile.childImage == null)
        {
            return string.Empty;
        }
        else
        {
            return tile.childImage.name;
        }
    }

    private void ResetMatchesIfWeAreOnNewRow(ref string lastTileName, ref int matchesInARow, int column)
    {
        if (column == 0)
        {
            matchesInARow = 0;
            lastTileName = string.Empty;
        }
    }
}
