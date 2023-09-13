using Game.Model;
using Game.View;
using Zenject;

namespace Game.Controller
{
    public class TileController
    {
        [Inject] private IGridModel _gridModel;
        [Inject] private IGridView _gridView;
        
        public void InitTiles()
        {
            for (int i = 0; i < _gridModel.Width * _gridModel.Height; i++)
            {
                var cell = _gridModel.GetCellModel(i);
                _gridView.CreateTile(cell.Id, cell.X, cell.Y,GetModelType(cell.TileModel.TileType));
            }
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

        public void MoveTile(CellModel neighbourUpCellView,CellModel destinationCellView)
        {
            var tile = neighbourUpCellView.TileModel;
            if (tile.TileType == 0) return;
            _gridView.MoveTile(neighbourUpCellView.Id,destinationCellView.Id);
            // tile.MoveTile(destinationCellView.transform);
            destinationCellView.SetTileModel(tile);
            neighbourUpCellView.SetTileModel(null);
            var destinationCellViewNeighbourDown = destinationCellView.GetNeighbour(Direction.Down);
            if (destinationCellViewNeighbourDown == null) return;
            if (destinationCellViewNeighbourDown.TileModel != null) return;
            MoveTile(destinationCellView,destinationCellViewNeighbourDown);
        }
    }
}