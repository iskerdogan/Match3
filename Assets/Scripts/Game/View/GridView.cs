using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game.View
{
    public interface IGridView
    {
        void InitGrid(int width, int height);
        void CreateCell(int id,int width,int height);
        void CreateTile(int cellId, int width, int height, TileType tileType);
        void SpawnTile(int width, TileType tileType,int destinationCellId);
        void MoveTile(int neighbourUpCellId,int destinationCellId);
        void SetDelegate(IGridViewDelegate gridViewDelegate);
        void PlayMatchAnimation(int id);
        void PlayMismatchAnimation(int id);
        void Test(int id);
    }   
    
    //Controllera ulaşmak için
    public interface IGridViewDelegate
    {
        void OnCellClicked(int id);
    }

    public class GridView:MonoBehaviour,IInitializable,IDisposable,IGridView
    {
        [SerializeField] private CellView _cellViewPrefab;
        [SerializeField] private TileView _tileViewPrefab;
        [SerializeField] private Camera _camera;
        
        [Inject] private InputManager _inputManager;
        [Inject] private TileMatchAnimation _tileMatchAnimation;
        [Inject] private TileMismatchAnimation _tileMismatchAnimation;
        [Inject] private TileSpawnerView _tileSpawnerView;

        private IGridViewDelegate _gridViewDelegate; //Controllera ulaşmak için

        private GameObject _cellParent;
        private CellView[] _cellViews;
        private bool _isCameraNull;
        
        public void Initialize()
        {
            Subscribe();
            _isCameraNull = _camera == null;
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

        public void InitGrid(int width, int height)
        {
            _cellViews = new CellView[width * height];
            _cellParent = new GameObject("TileParent");
        }

        public void CreateCell(int id,int width,int height)
        {
            CellView cellView = Instantiate(_cellViewPrefab, Vector3.zero,Quaternion.identity, _cellParent.transform);
            cellView.InitCell(id,width,height);
            _cellViews[id] = cellView;
        }
        
        public void CreateTile(int cellId,int width,int height,TileType tileType)
        {
            TileView tileView = Instantiate(_tileViewPrefab, Vector3.zero,Quaternion.identity, _cellViews[cellId].transform);
            tileView.InitTile(width, height, tileType);
            _cellViews[cellId].SetTileView(tileView);
        }

        public void SpawnTile(int width,TileType tileType,int destinationCellId)
        {
            TileView tileView = Instantiate(_tileViewPrefab, Vector3.zero,Quaternion.identity);
            tileView.InitTile((int)_tileSpawnerView.GetSpawnTilePosition(width).x, (int)_tileSpawnerView.GetSpawnTilePosition(width).y,tileType);
            var destinationCell = _cellViews[destinationCellId];
            destinationCell.SetTileView(tileView);
            MoveTile(tileView, destinationCell);
        }
        
        public Vector3 GetTileSpawnerPosition(int id)
        {
            return _cellViews[id].transform.position + Vector3.up;
        }

        public void MoveTile(int neighbourUpCellId, int destinationCellId)
        {
            var neighbourUpCell = _cellViews[neighbourUpCellId];
            var destinationCell = _cellViews[destinationCellId];
            neighbourUpCell.TileView.MoveTile(destinationCell.transform);
            destinationCell.SetTileView(neighbourUpCell.TileView);
            neighbourUpCell.SetTileView(null);
        }
        
        public void MoveTile(TileView tileView, CellView destinationCell)
        {
            tileView.MoveTile(destinationCell.transform);
            destinationCell.SetTileView(tileView);
            // destinationCell.SetTileView(tileView);
        }

        private void Subscribe()
        {
            _inputManager.Clicked += Clicked;
        }

        private void Unsubscribe()
        {
            _inputManager.Clicked -= Clicked;
        }

        public void PlayMatchAnimation(int id)
        {
            var tileView = _cellViews[id].TileView;
            if (!tileView) return;
            _tileMatchAnimation.Execute(tileView.transform,()=> Destroy(tileView.gameObject));
        }
        
        public void PlayMismatchAnimation(int id)
        {
            var tileView = _cellViews[id].TileView;
            if (!tileView) return;
            _tileMismatchAnimation.Execute(tileView.transform);
        }

        public void Test(int id)
        {
            Debug.Log(id);
        }

        private void Clicked()
        {
            if (_isCameraNull) return;
            var rayMouse = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit)) return;
            if (!hit.transform.TryGetComponent(out CellView cell)) return;
            _gridViewDelegate.OnCellClicked(cell.Id);
        }
    }
}