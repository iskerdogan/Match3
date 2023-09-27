using Game.Model;
using Game.View;
using Zenject;

namespace Game.Controller
{
    public class TileSpawnerController
    {
        [Inject] private ITileSpawnerView _tileSpawnerView;
        [Inject] private IGridModel _gridModel;

        public void InitSpawner()
        {
            var gridWidth = _gridModel.Width;
            var gridHeight = _gridModel.Height;
            _tileSpawnerView.InitSpawnerArray(gridWidth);
            for (int i = 0; i < _gridModel.Width; i++)
            {
                var cellModel = _gridModel.GetCellModel(i, gridHeight-1);
                _tileSpawnerView.CreateSpawner(i, cellModel.Id);
            }
        }
        
    }
}
