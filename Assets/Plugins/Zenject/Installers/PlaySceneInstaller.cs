using UnityEngine;
using Zenject;

public class PlaySceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ICellManager>().To<CellManager>()
       .FromComponentOn(FindObjectOfType<CellManager>().gameObject).AsSingle().NonLazy();

        Container.Bind<ILevelManager>().To<LevelManager>()
            .FromComponentOn(FindObjectOfType<LevelManager>().gameObject).AsSingle().NonLazy();

        Container.Bind<IObjectPoolManager>().To<ObjectPoolManager>()
            .FromComponentOn(FindObjectOfType<ObjectPoolManager>().gameObject).AsSingle().NonLazy();

        Container.Bind<ITileManager>().To<TileManager>()
            .FromComponentOn(FindObjectOfType<TileManager>().gameObject).AsSingle().NonLazy();

        Container.Bind<IGridController>().To<GridController>()
            .FromComponentOn(FindObjectOfType<GridController>().gameObject).AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<MatchHandler>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TileMovementHandler>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TileRemover>().AsSingle().NonLazy();

        Container.Bind<IGameLoopHandler>().To<GameLoopHandler>()
            .FromComponentOn(FindObjectOfType<GameLoopHandler>().gameObject).AsSingle().NonLazy();
    }
}