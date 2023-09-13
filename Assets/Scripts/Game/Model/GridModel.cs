using System;
using System.Linq;
using Zenject;

namespace Game.Model
{
    // public struct GridData
    // {
    //     public int Width { get; set; }
    //     public int Height { get; set; }
    //     public float Distance { get; set; }
    //     public TileData[] TileDatas { get; set; }
    //     
    //     public TileSpawnerData[] tileSpawnerData { get; set; }
    //
    // }
    //
    // public struct TileData
    // {
    //     public int Id;
    //     public int Width;
    //     public int Height;
    //     public TileType TileType;
    // }
    // public struct TileSpawnerData
    // {
    //     public SpawnTileData[] spawnTileDatas;
    // }
    //
    // public struct SpawnTileData
    // {
    //     public TileType type;
    //     public int spawnProbability;
    //     
    // }

    public interface IGridModel
    {
        void SetData(string[] data);
        CellModel GetCellModel(int x,int y);
        CellModel GetCellModel(int id);
        int Width { get;}
        int Height { get;}

    }
    public class GridModel : IGridModel,IInitializable
    {
        public int Width { get; private set; }
        public int Height { get;private set; }
        private CellModel[] _cellModels;
        
        public void Initialize()
        {
            
        }
        
        public void SetData(string[] data)
        {
            Width = data[0].Length;
            Height = data.Length;
            var cellCount = Width * Height;
            _cellModels = new CellModel[cellCount];
            SetCell();
            // SetCellNeighbour();
            SetTile(data);
        }

        private void SetCell()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var cellModel = new CellModel(this);
                    var id = Width * y + x;
                    cellModel.SetData(id, x, y);
                    _cellModels[id] = cellModel;
                }
            }
        }

        private void SetTile(string[] data)
        {
            for (int i = 0; i < Height; i++)
            {
                var line = data[i].ToArray();
                for (int j = 0; j < Width; j++)
                {
                    var tileModel = new TileModel();
                    var cellModel = _cellModels[Width * i + j];
                    var tileType = line[j].ToString();
                    tileModel.SetData(Int16.Parse(tileType));
                    cellModel.SetTileModel(tileModel);
                }
            }
        }

        // private void SetCellNeighbour()
        // {
        //     for (int i = 0; i < _cellModels.Length; i++)
        //     {
        //         for (int j = 0; j < Enum.GetNames(typeof(Direction)).Length; j++)
        //         {
        //             _cellModels[i].SetNeighbours((Direction)j,GetCellNeighbourId((Direction)j, _cellModels[i].Id,_cellModels[i].X,_cellModels[i].Y,Width,Height));
        //         }
        //     }
        // }
        //
        // private CellModel GetCellNeighbourId(Direction neighbour,int cellId,int cellX,int cellY,int gridWidth,int gridHeight)
        // {
        //     CellModel result;
        //     switch (neighbour)
        //     {
        //         case Direction.Up:
        //             if (cellY == gridHeight - 1) return null;
        //             result = _cellModels[cellId + gridWidth];
        //             break;
        //         case Direction.Down:
        //             if (cellY == 0) return null;
        //             result =  _cellModels[cellId - gridWidth];
        //             break;
        //         case Direction.Left:
        //             if (cellX == 0) return null;
        //             result =  _cellModels[cellId - 1];
        //             break;
        //         case Direction.Right:
        //             if (cellX == gridWidth - 1) return null;
        //             result =  _cellModels[cellId + 1];
        //             break;
        //         default:
        //             result = null;
        //             break;
        //     }
        //     return result;
        // }
        
        public CellModel GetCellModel(int x, int y)
        {
            if (!(x >= 0 && x < Width) || !(y >= 0 && y < Height)) return null;
            return _cellModels[Width * y + x];
        }

        public CellModel GetCellModel(int id)
        {
            return _cellModels[id];
        }


    }
}