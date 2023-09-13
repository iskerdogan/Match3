using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Model;
using Game.View;
using Zenject;

namespace Game.Controller
{
    public class GridController : IInitializable,IGridViewDelegate
    {
        [Inject] private IGridModel _gridModel;
        [Inject] private IGridView _gridView;
        [Inject] private ITileMatchAnimation _tileMatchAnimation;
        [Inject] private CellController _cellController;
        [Inject] private TileController _tileController;
        
        //Controllera ulaşmak için
        public int Test { get; set; }
        
        private List<CellModel> _cellMatch = new List<CellModel>();
        private bool[] _cellMatchBool;

        public void Initialize()
        {
            _gridView.SetDelegate(this);
            CreateGird();
        }

        private void CreateGird()
        {
            _gridView.InitGrid(_gridModel.Width , _gridModel.Height);
            _cellMatchBool = new bool[_gridModel.Width * _gridModel.Height];
            _cellController.InitCells();
            _tileController.InitTiles();
        }
        
         private void CheckTileTypesRecursive(int id)
         {
             var cellModel = _gridModel.GetCellModel(id);
             if (_cellMatchBool[cellModel.Id]) return; 
             _cellMatchBool[cellModel.Id] = true;
             
             for (int i = 0; i < 4; i++)
             {
                 var neighbour = cellModel.GetNeighbour((Direction)i);
                 if (cellModel.CheckNeighbourMatch((Direction)i) && !_cellMatchBool[neighbour.Id])
                 {
                     _cellMatch.Add(neighbour);
                     CheckTileTypesRecursive(neighbour.Id);
                 }
             }
        }

         public void OnCellClicked(int id)
         {
             var cellModel = _gridModel.GetCellModel(id);
             if (cellModel.TileModel == null) return;
             if (cellModel.TileModel.TileType == 0) return;
             ResetToBoolArray();
             _cellMatch.Clear();
             _cellMatch.Add(cellModel); 
             CheckTileTypesRecursive(id);
             if (_cellMatch.Count < 3)
             {
                 //TODO
                 PlayMismatchAnimations();
                 return;
             }
             DestroyEqualsTile();
         }

         private void PlayMismatchAnimations()
         {
             for (int i = 0; i < _cellMatch.Count; i++)
             {
                 _gridView.PlayMismatchAnimation(_cellMatch[i].Id);
             }
         }

         private async void DestroyEqualsTile()
        {
            for (int i = 0; i < _cellMatch.Count; i++)
            {
                _cellMatch[i].SetTileModel(null);
                _gridView.PlayMatchAnimation(_cellMatch[i].Id);
            }

            await UniTask.WaitForSeconds(_tileMatchAnimation.Duration);
            
            for (int i = 0; i < _cellMatch.Count; i++)
            {
                CheckNeighbourUp(_cellMatch[i]);
            }
        }

        private void CheckNeighbourUp(CellModel cellModel)
        {
            var neighbourUp = cellModel.GetNeighbour(Direction.Up);
            if (neighbourUp == null)
            {
                cellModel.SetTileModel(null);
                return;
            }
            if (neighbourUp.TileModel == null) return; //TODO şu an yeni tile generate etmediği için hata fırlatmasını önlüyor
            // if (_cellMatch.Contains(neighbourUp)) return;
            if (_cellMatchBool[neighbourUp.Id]) return; 
            _tileController.MoveTile(neighbourUp,cellModel);
            CheckNeighbourUp(neighbourUp);
        }
        
        private void ResetToBoolArray()
        {
            for (int i = 0; i < _cellMatchBool.Length; i++)
            {
                _cellMatchBool[i] = false;
            }
        }

    }
}
