using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Game.View
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public interface IGridView
    {
        void SetCellsArray(int width, int height);
        void CreateParentObject();
        void CreateCell(int id,int width,int height,View.TileType type);
        void SetNeighbour(int gridWidth,int gridHeight);
        void ClearTilesTest();
        void SetDelegate(IGridViewDelegate gridViewDelegate);

    }   
    
    //Controllera ulaşmak için
    public interface IGridViewDelegate
    {
        public int Test { get; set; }
    }

    public class GridView:MonoBehaviour,IInitializable,IDisposable,IGridView
    {
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private Tile tilePrefab;
        
        [Inject] private InputManager _inputManager;

        private GameObject _cellParent;
        private IGridViewDelegate _gridViewDelegate; //Controllera ulaşmak için
        private List<Cell> _cellMatch = new List<Cell>();
        private bool[] _cellMatchBool;

        private Cell[] _cells;
        
        public void Initialize()
        {
            Subscribe();
        }

        //Controllera ulaşmak için
        public void SetDelegate(IGridViewDelegate gridViewDelegate)
        {
            _gridViewDelegate = gridViewDelegate;
        }
        
        public void Dispose()
        {
            Unsubscribe();
        }

        public void SetCellsArray(int width, int height)
        {
            _cells = new Cell[width * height];
            _cellMatchBool = new bool[width * height];
        }

        public void CreateParentObject()
        {
            _cellParent = new GameObject("TileParent");
        }

        public void CreateCell(int id,int width,int height,View.TileType type)
        {
            var cellPosition = Vector3.zero;
            Cell cell = Instantiate(cellPrefab, Vector3.zero,Quaternion.identity, _cellParent.transform);
            Tile tile = Instantiate(tilePrefab, Vector3.zero,Quaternion.identity, cell.transform);
            tile.TileType = type;
            cell.Id = id;
            cell.Width = width;
            cell.Height = height;
            cellPosition = new Vector3(width, height, 0);
            cell.transform.position = cellPosition;
            cell.name = "Tile " + "(" + width + " , " + height + ")";
            tile.transform.position = cellPosition;
            cell.CurrentTile = tile;
            _cells[cell.Id] = cell;
        }
        
        public void SetNeighbour(int gridWidth,int gridHeight)
        {
            foreach (var cell in _cells)
            {
                cell.neighbourUp = GetNeighbour(Direction.Up, cell,gridWidth,gridHeight);
                cell.neighbourDown = GetNeighbour(Direction.Down, cell,gridWidth,gridHeight);
                cell.neighbourRight = GetNeighbour(Direction.Right, cell,gridWidth,gridHeight);
                cell.neighbourLeft = GetNeighbour(Direction.Left,cell,gridWidth,gridHeight);
            }
        }

        private Cell GetNeighbour(Direction neighbour,Cell cell,int gridWidth,int gridHeight)
        {
            Cell result = null;
            switch (neighbour)
            {
                case Direction.Up:
                    if (cell.Height == gridHeight - 1) return null;
                    // result = _cells.Find(x => x.Id == cell.Id + gridData.Width);
                    result = _cells[cell.Id + gridWidth];
                    break;
                case Direction.Down:
                    if (cell.Height == 0) return null;
                    // result = _cells.Find(x => x.Id == cell.Id - gridData.Width);
                    result = _cells[cell.Id - gridWidth];
                    break;
                case Direction.Left:
                    if (cell.Width == 0) return null;
                    // result = _cells.Find(x => x.Id == cell.Id - 1);
                    result = _cells[cell.Id - 1];
                    break;
                case Direction.Right:
                    if (cell.Width == gridWidth - 1) return null;
                    // result = _cells.Find(x => x.Id == cell.Id + 1);
                    result = _cells[cell.Id + 1];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(neighbour), neighbour, null);
            }
            return result;
        }

        public void ClearTilesTest()
        {
            _cells = null;
            // if (!tileParent) return;
            // Destroy(tileParent);
        }

        private void Subscribe()
        {
            _inputManager.Clicked += Clicked;
        }

        private void Unsubscribe()
        {
            _inputManager.Clicked -= Clicked;
        }
        
        private async Task CheckTileType(Cell cell) 
        {
            if (cell.CheckNeighbourUp() && !_cellMatchBool[cell.neighbourUp.Id])
            {
                _cellMatchBool[cell.neighbourUp.Id] = true;
                _cellMatch.Add(cell.neighbourUp);
                await CheckTileType(cell.neighbourUp);
            }
            if (cell.CheckNeighbourDown()&& !_cellMatchBool[cell.neighbourDown.Id])
            {
                _cellMatchBool[cell.neighbourDown.Id] = true;
                _cellMatch.Add(cell.neighbourDown);
                await CheckTileType(cell.neighbourDown);
            }
            if (cell.CheckNeighbourleft()&& !_cellMatchBool[cell.neighbourLeft.Id])
            {
                _cellMatchBool[cell.neighbourLeft.Id] = true;
                _cellMatch.Add(cell.neighbourLeft);
                await CheckTileType(cell.neighbourLeft);
            }
            if (cell.CheckNeighbourRigth()&& !_cellMatchBool[cell.neighbourRight.Id])
            {
                _cellMatchBool[cell.neighbourRight.Id] = true;
                _cellMatch.Add(cell.neighbourRight);
                await CheckTileType(cell.neighbourRight);
            }

            if (!_cellMatchBool[cell.Id])
            {
                _cellMatchBool[cell.Id] = true;
                _cellMatch.Add(cell);
            }
        }

        private async Task DestroyEqualsTile()
        {
            Task[] tasks = new Task[_cellMatch.Count];
            GameObject[] destroyObjects = new GameObject[_cellMatch.Count];
            for (int i = 0; i < _cellMatch.Count; i++)
            {
                var temp = i;
                var currentTile = _cellMatch[temp].CurrentTile.gameObject;
                _cellMatch[temp].CurrentTile = null;
                destroyObjects[temp] = currentTile;
                tasks[temp] = DestroyAnimations(currentTile);
            }

            await Task.WhenAll(tasks);
            
            for (int i = 0; i < _cellMatch.Count; i++)
            {
                var temp = i;
                Destroy(destroyObjects[temp]);
                CheckNeighbourUp(_cellMatch[temp]);
            }
        }

        private async Task DestroyAnimations(GameObject currentTile)
        {
            await currentTile.transform.DOScale(0, .5f).SetEase(Ease.InBack).AsyncWaitForCompletion();
        }

        private void CheckNeighbourUp(Cell cell)
        {
            var neighbourUp = cell.neighbourUp;
            if (!neighbourUp)
            {
                cell.CurrentTile = null;
                return;
            }
            if (!neighbourUp.CurrentTile) return; //TODO şu an yeni tile generate etmediği için hata fırlatmasını önlüyor
            if (_cellMatch.Contains(neighbourUp)) return;
            MoveTile(neighbourUp,cell);
            CheckNeighbourUp(neighbourUp);
        }

        private void MoveTile(Cell neighbourUpCell,Cell destinationCell)
        {
            var tile = neighbourUpCell.CurrentTile;
            tile.MoveTile(destinationCell.transform);
            destinationCell.CurrentTile = tile;
            neighbourUpCell.CurrentTile = null;
            if (!destinationCell.neighbourDown) return;
            if (destinationCell.neighbourDown.CurrentTile != null) return;
            MoveTile(destinationCell,destinationCell.neighbourDown);
        }

        private void ResetToBoolArray()
        {
            for (int i = 0; i < _cellMatchBool.Length; i++)
            {
                _cellMatchBool[i] = false;
            }
        }

        private async void Clicked()
        {
            if (Camera.main == null) return;
            var rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit)) return;
            if (!hit.transform.TryGetComponent(out Cell cell)) return;
            if (!cell.CurrentTile) return;
            if (cell.CurrentTile.TileType == TileType.Empty) return;
            ResetToBoolArray();
            _cellMatch.Clear();
            await CheckTileType(cell);
            // if (_cellMatch.Count >= 3) DestroyEqualsTile(); //TODO
            await DestroyEqualsTile();
            Debug.Log(_cellMatch.Count);
        }
    }
}