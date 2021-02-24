using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using UnityEngine.Assertions;

public class ObjectPoolManager : MonoBehaviour, IObjectPoolManager
{
    [SerializeField] private TileImagePool tileImagePool;
    [SerializeField] private CellPool cellPool;
    [SerializeField] private TileControllerPool tileControllerPool;
    private IGridController gridController;

    [Inject]
    public void Init(IGridController gridController)
    {
        this.gridController = gridController;
    }

    void Start()
    {
        CreatePools();
        StartCoroutine(gridController.CreateBoardRoutine);

        Assert.IsNotNull(tileImagePool.Prefabs);
        Assert.IsNotNull(tileImagePool.parentWhenPooled);

        Assert.IsNotNull(cellPool.Prefabs);
        Assert.IsNotNull(cellPool.parentWhenPooled);

        Assert.IsNotNull(tileControllerPool.Prefab);
        Assert.IsNotNull(tileControllerPool.parentWhenPooled);
    }

    public GameObject GetPooledTile()
    {
        for (int i = 0; i < tileControllerPool.tileControllers.Count; i++)
        {
            if (!tileControllerPool.tileControllers[i].activeSelf)
                return tileControllerPool.tileControllers[i];
        }
        return CreateAndPoolObject(tileControllerPool.Prefab, tileControllerPool.parentWhenPooled, tileControllerPool.tileControllers);
    }

    public GameObject GetPooledCell(string PrefabName)
    {
        for (int i = 0; i < cellPool.cells.Count; i++)
        {
            if (!cellPool.cells[i].activeSelf && cellPool.cells[i].name.StartsWith(PrefabName))
                return cellPool.cells[i];
        }
        var prefab = cellPool.cells.Where(x => x.name == PrefabName).First();
        return CreateAndPoolObject(prefab, cellPool.parentWhenPooled, cellPool.cells);
    }

    public GameObject GetPooledTileImage(string PrefabName)
    {
        for (int i = 0; i < tileImagePool.tileImages.Count; i++)
        {
            if (!tileImagePool.tileImages[i].activeSelf && tileImagePool.tileImages[i].name.StartsWith(PrefabName))
                return tileImagePool.tileImages[i];
        }
        var prefab = tileImagePool.tileImages.Where(x => x.name == PrefabName).First();
        return CreateAndPoolObject(prefab, tileImagePool.parentWhenPooled, tileImagePool.tileImages);
    }

    private GameObject CreateAndPoolObject(GameObject prefab, Transform pooledParent, List<GameObject> pool)
    {
        var tmp = Instantiate(prefab, pooledParent);
        tmp.name = prefab.name;
        tmp.gameObject.SetActive(false);
        pool.Add(tmp);
        return tmp;
    }

    private void CreatePools()
    {
        for (int i = 0; i < tileImagePool.Prefabs.Length; i++)
        {
            for (int j = 0; j < tileImagePool.Spawn; j++)
                CreateAndPoolObject(tileImagePool.Prefabs[i], tileImagePool.parentWhenPooled, tileImagePool.tileImages);
        }

        for (int i = 0; i < cellPool.Prefabs.Length; i++)
        {
            for (int j = 0; j < cellPool.Spawn; j++)
                CreateAndPoolObject(cellPool.Prefabs[i].gameObject, cellPool.parentWhenPooled, cellPool.cells);
        }

        for (int j = 0; j < tileControllerPool.Spawn; j++)
            CreateAndPoolObject(tileControllerPool.Prefab, tileControllerPool.parentWhenPooled, tileControllerPool.tileControllers);
    }

    public void PoolTileImageController(TileImageController childImage)
    {
        childImage.transform.SetParent(tileImagePool.parentWhenPooled,false);
        childImage.donutImage.color=new Color(255, 255, 255, 255);
        childImage.gameObject.SetActive(false);
    }

    public void PoolCell(GameObject cell)
    {
        cell.transform.SetParent(cellPool.parentWhenPooled,false);
        cell.SetActive(false);
    }

    public void PoolTileController(TileController tileController)
    {
        tileController.transform.SetParent(tileImagePool.parentWhenPooled, false);
        tileController.gameObject.SetActive(false);
    }

    public void PoolTileCollection(List<TileController> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].childImage != null)
            {
                PoolTileImageController(tiles[i].childImage);
            }

            tiles[i].childImage = null;
            tiles[i].x = 0;
            tiles[i].y = 0;

            PoolTileController(tiles[i]);
        }
        tiles.Clear();
    }

    [System.Serializable]
    public class TileImagePool
    {
        public GameObject[] Prefabs;
        public Transform parentWhenPooled;
        [HideInInspector] public List<GameObject> tileImages = new List<GameObject>();
        public int Spawn;
    }

    [System.Serializable]
    public class CellPool
    {
        public Cell[] Prefabs;
        public Transform parentWhenPooled;
        [HideInInspector] public List<GameObject> cells = new List<GameObject>();
        public int Spawn;
    }

    [System.Serializable]
    public class TileControllerPool
    {
        public GameObject Prefab;
        public Transform parentWhenPooled;
        [HideInInspector] public List<GameObject> tileControllers = new List<GameObject>();
        public int Spawn;
    }
}
