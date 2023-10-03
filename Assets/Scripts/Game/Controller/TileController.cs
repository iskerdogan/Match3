using System.Security.Cryptography;
using Game.Model;
using Game.View;
using Zenject;

namespace Game.Controller
{
    public class TileController
    {
        [Inject] private IGridModel _gridModel;
        [Inject] private IGridView _gridView;
        [Inject] private TileSpawnerController _tileSpawnerController;
        
        public void InitTiles()
        {
            for (int i = 0; i < _gridModel.Width * _gridModel.Height; i++)
            {
                var cell = _gridModel.GetCellModel(i);
                _gridView.CreateTile(cell.Id, cell.X, cell.Y,GetModelType(cell.TileModel.TileType));
            }
        }
        
        public void SpawnTile(CellModel matchCell)
        {
            var startCell = _gridModel.GetCellModel(matchCell.X, _gridModel.Height - 1);
            if (startCell.TileModel != null) return;
            CheckNeighbourDown(startCell);
        }
        
        private void CheckNeighbourDown(CellModel cellModel)
        {
            var neighbourDown = cellModel.GetNeighbour(Direction.Down);
            if (neighbourDown == null)
            {
                MoveTile(cellModel);
                return;
            }
            if (neighbourDown.TileModel != null)
            {
                MoveTile(cellModel);
                return;
            }
            CheckNeighbourDown(neighbourDown);
        }
        
        private TileType GetModelType(int tileType)
        {
            TileType result = TileType.Empty;
            switch (tileType)
            {
                case 0:
                    result = TileType.Empty;
                    break;
                case 1: 
                    result = TileType.Red;
                    break;
                case 2: 
                    result = TileType.Green;
                    break;
                case 3: 
                    result = TileType.Blue;
                    break;
            }
            return result;
        }

        public void MoveTile(CellModel neighbourUpCell,CellModel destinationCell)
        {
            var tile = neighbourUpCell.TileModel;
            if (tile?.TileType == 0) return;
            neighbourUpCell.SetTileModel(null);
            destinationCell.SetTileModel(tile);
            _gridView.MoveTile(neighbourUpCell.Id,destinationCell.Id);
            var destinationCellViewNeighbourDown = destinationCell.GetNeighbour(Direction.Down);
            if (destinationCellViewNeighbourDown == null) return;
            if (destinationCellViewNeighbourDown.TileModel != null) return;
            MoveTile(destinationCell,destinationCellViewNeighbourDown);
        }
        private void MoveTile(CellModel cellModel)
        {
            // var type = RandomNumberGenerator.GetInt32(1, 4);
            var type = _tileSpawnerController.GetSpawnTileType();
            _gridView.SpawnTile(cellModel.X,GetModelType(type),cellModel.Id);
            cellModel.SetTileModel(new TileModel());
            cellModel.TileModel.SetData(type);
        }
    }
}