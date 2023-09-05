using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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

    public class GridView:MonoBehaviour,IInitializable,IDisposable
    {
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private Tile tilePrefab;
        
        [Inject] private InputManager _inputManager;

        private GameObject _cellParent;
        private List<Cell> _cellMatch = new List<Cell>();

        private Cell[] _cells;
        
        public void Initialize()
        {
            Subscribe();
        }
        
        public void Dispose()
        {
            Unsubscribe();
        }

        public void SetCellsArray(int width, int height) => _cells = new Cell[width * height];
        public void CreateParentObject() => _cellParent = new GameObject("TileParent");
        public void CreateCell(int id,int width,int height,View.TileType type)
        {
            var cellPosition = Vector3.zero;
            //Quaternion unity.math kütüphanesini kabul etmiyor neden?
            Cell cell = Instantiate(cellPrefab, Vector3.zero,new Quaternion(0,0,0,0), _cellParent.transform);
            Tile tile = Instantiate(tilePrefab, Vector3.zero,new Quaternion(0,0,0,0), cell.transform);
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
        
        private void CheckTileType(Cell cell) {
            if (cell.CheckNeighbourUp() && !_cellMatch.Contains(cell.neighbourUp))
            {
                _cellMatch.Add(cell.neighbourUp);
                CheckTileType(cell.neighbourUp);
            }
            if (cell.CheckNeighbourDown()&& !_cellMatch.Contains(cell.neighbourDown))
            {
                _cellMatch.Add(cell.neighbourDown);
                CheckTileType(cell.neighbourDown);
            }
            if (cell.CheckNeighbourleft()&& !_cellMatch.Contains(cell.neighbourLeft))
            {
                _cellMatch.Add(cell.neighbourLeft);
                CheckTileType(cell.neighbourLeft);
            }
            if (cell.CheckNeighbourRigth()&& !_cellMatch.Contains(cell.neighbourRight))
            {
                _cellMatch.Add(cell.neighbourRight);
                CheckTileType(cell.neighbourRight);
            }

            if (!_cellMatch.Contains(cell))
            {
                _cellMatch.Add(cell);
            }
        }

        private void DestroyEqualsTile()
        {
            for (int i = 0; i < _cellMatch.Count; i++)
            {
                var temp = i;
                var currentTile = _cellMatch[temp].CurrentTile;
                _cellMatch[temp].CurrentTile = null;
                Destroy(currentTile.gameObject);
                CheckNeighbourUp(_cellMatch[temp]);
            }
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
            tile.transform.SetParent(null);
            tile.transform.position = destinationCell.transform.position;
            tile.transform.SetParent(destinationCell.transform);
            destinationCell.CurrentTile = tile;
            neighbourUpCell.CurrentTile = null;
        }

        private void Clicked()
        {
            if (Camera.main == null) return;
            var rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit)) return;
            if (!hit.transform.TryGetComponent(out Cell cell)) return;
            if (!cell.CurrentTile) return;
            if (cell.CurrentTile.TileType == TileType.Empty) return;
            _cellMatch.Clear();
            CheckTileType(cell);
            // if (_cellMatch.Count >= 3) DestroyEqualsTile(); //TODO
            DestroyEqualsTile();
            Debug.Log(_cellMatch.Count);
        }
    }
}