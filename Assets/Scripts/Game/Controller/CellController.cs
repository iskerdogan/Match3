using System.Threading.Tasks;
using Game.Model;
using Game.View;
using Zenject;

namespace Game.Controller
{
    public class CellController
    {
        [Inject] private IGridModel _gridModel;
        [Inject] private IGridView _gridView;
        
        public void InitCells()
        {
            for (int i = 0; i < _gridModel.Width * _gridModel.Height; i++)
            {
                var cell = _gridModel.GetCellModel(i);
                _gridView.CreateCell(cell.Id, cell.X, cell.Y);
            }
        }

        // public async Task DestroyTile(int cellId)
        // {
        //     _gridView.
        // }
        
    }
}