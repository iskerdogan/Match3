using System;
using Game.Controller;
using Game.Model;
using Game.View;
using UnityEngine;
using Zenject;
using TileType = Game.View.TileType;

namespace Game.Injection
{

    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GridView _gridView;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private LevelConfig _levelConfig;

        public override void InstallBindings()
        {
            // base.InstallBindings();
            var gridData = GetGirdData();
            Container.BindInterfacesAndSelfTo<GridModel>().AsSingle().WithArguments(gridData);
            Container.BindInterfacesAndSelfTo<GridController>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputManager>().FromInstance(_inputManager).AsSingle();
            Container.BindInterfacesAndSelfTo<GridView>().FromInstance(_gridView).AsSingle();
        }

        private GridData GetGirdData()
        {
            var gridData = new GridData();
            gridData.Distance = _levelConfig.distance;
            gridData.Height = _levelConfig.height;
            gridData.Width = _levelConfig.width;
            var tileDatas = new Model.TileData[gridData.Height * gridData.Width];
            for (int i = 0; i < tileDatas.Length; i++)
            {
                tileDatas[i].Height = _levelConfig.tileDatas[i].Coordinate.y;
                tileDatas[i].Width = _levelConfig.tileDatas[i].Coordinate.x;
                tileDatas[i].Id = _levelConfig.tileDatas[i].Id;
                tileDatas[i].TileType = GetModelType(_levelConfig.tileDatas[i].TileType);
            }

            gridData.TileDatas = tileDatas;
            return gridData;
        }

        private Model.TileType GetModelType(View.TileType tileType)
        {
            return tileType switch
            {
                TileType.Red => Model.TileType.Red,
                TileType.Green => Model.TileType.Green,
                TileType.Blue => Model.TileType.Blue,
                TileType.Empty => Model.TileType.Empty,
                _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
            };
        }
    }
}