using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPoolManager
{
    GameObject GetPooledCell(string PrefabName);
    GameObject GetPooledTileImage(string PrefabName);
    GameObject GetPooledTile();
    void PoolTileImageController(TileImageController childImage);
    void PoolCell(GameObject gameObject);
    void PoolTileController(TileController tileController);
    void PoolTileCollection(List<TileController> tiles);
}

