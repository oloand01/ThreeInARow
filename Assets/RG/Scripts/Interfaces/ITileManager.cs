using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileManager 
{
    IEnumerator CreateTilesRoutine { get; }
    List<TileController> tiles { get; set; }
    int ConstraintCount { get; }        
    void PoolAllTiles();
    List<TileImageController> ReparentTileImages(List<TileImageController> fallDownCache);
    void PoolMatchedTileImages(List<TileImageController> matchingImagesCache);
}
