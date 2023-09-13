using System;
using Game.Controller;
using Game.Model;
using Game.View;
using UnityEngine;
using Zenject;

namespace Game.Injection
{

    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GridView _gridView;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private TileMatchAnimation _tileMatchAnimation;

        public override void InstallBindings()
        {
            // base.InstallBindings();
            // var gridData = GetGirdData();
            Container.BindInterfacesAndSelfTo<GridModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridView>().FromInstance(_gridView).AsSingle();
            Container.BindInterfacesAndSelfTo<TileMatchAnimation>().FromInstance(_tileMatchAnimation).AsSingle();
            Container.BindInterfacesAndSelfTo<CellController>().AsSingle();
            Container.BindInterfacesAndSelfTo<TileController>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputManager>().FromInstance(_inputManager).AsSingle();
            Container.BindInterfacesAndSelfTo<LevelView>().AsSingle();
        }

        // private GridData GetGirdData()
        // {
        //     var gridData = new GridData();
        //     gridData.Distance = _levelConfig.distance;
        //     gridData.Height = _levelConfig.height;
        //     gridData.Width = _levelConfig.width;
        //     var tileDatas = new Model.TileData[gridData.Height * gridData.Width];
        //     for (int i = 0; i < tileDatas.Length; i++)
        //     {
        //         tileDatas[i].Height = _levelConfig.tileDatas[i].Coordinate.y;
        //         tileDatas[i].Width = _levelConfig.tileDatas[i].Coordinate.x;
        //         tileDatas[i].Id = _levelConfig.tileDatas[i].Id;
        //         tileDatas[i].TileType = GetModelType(_levelConfig.tileDatas[i].TileType);
        //         // var tileSpawnerData = new Model.TileSpawnerData[gridData.Width];
        //         // for (int j = 0; j < tileSpawnerData.Length; j++)
        //         // {
        //         //     var spawnTileData = new Model.SpawnTileData[Enum.GetNames(typeof(TileType)).Length-1];
        //         //     for (int x = 0; x < spawnTileData.Length; x++)
        //         //     {
        //         //         spawnTileData[x].type = GetModelType(_levelConfig.tileSpawnerData[j].spawnTileDatas[x].type);
        //         //         spawnTileData[x].spawnProbability = _levelConfig.tileSpawnerData[j].spawnTileDatas[x].spawnProbability;
        //         //     }
        //         //     tileSpawnerData[j].spawnTileDatas = spawnTileData;
        //         //     tileDatas[j].TileType = GetModelType(_levelConfig.tileDatas[j].TileType);
        //         // }
        //         // gridData.tileSpawnerData = tileSpawnerData;
        //     }
        //     gridData.TileDatas = tileDatas;
        //     return gridData;
        // }
        //
        // private Model.TileType GetModelType(View.TileType tileType)
        // {
        //     return tileType switch
        //     {
        //         TileType.Red => Model.TileType.Red,
        //         TileType.Green => Model.TileType.Green,
        //         TileType.Blue => Model.TileType.Blue,
        //         TileType.Empty => Model.TileType.Empty,
        //         _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
        //     };
        // }
    }
}