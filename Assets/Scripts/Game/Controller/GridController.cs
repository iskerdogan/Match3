using System;
using Game.Model;
using Game.View;
using Zenject;

namespace Game.Controller
{
    public class GridController : IInitializable,IGridViewDelegate
    {
        [Inject] private GridModel _gridModel;
        [Inject] private IGridView _gridView;
        
        //Controllera ulaşmak için
        public int Test { get; set; }

        public void Initialize()
        {
            _gridView.SetDelegate(this);
            CreateGird();
        }

        private void CreateGird()
        {
            var gridData = GetGridData();
            _gridView.ClearTilesTest();
            _gridView.SetCellsArray(gridData.Width,gridData.Height);
            _gridView.CreateParentObject();
            foreach (var tileData in gridData.TileDatas)
            {
                _gridView.CreateCell(tileData.Id,tileData.Width,tileData.Height,GetModelType(tileData.TileType));
            }
            _gridView.SetNeighbour(gridData.Width,gridData.Height);
        }
        

        private GridData GetGridData()
        {
            return _gridModel.GridData;
        }
        

        private View.TileType GetModelType(Model.TileType tileType)
        {
            return tileType switch
            {
                Model.TileType.Red => View.TileType.Red,
                Model.TileType.Green => View.TileType.Green,
                Model.TileType.Blue => View.TileType.Blue,
                Model.TileType.Empty => View.TileType.Empty,
                _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
            };
        }

    }
}
