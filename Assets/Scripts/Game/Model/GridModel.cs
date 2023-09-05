using System;

namespace Game.Model
{
    public enum TileType
    {
        Red,
        Green,
        Blue,
        Empty
    }
    public struct GridData
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public float Distance { get; set; }
        public TileData[] TileDatas { get; set; }
    }
    
    public struct TileData
    {
        public int Id;
        public int Width;
        public int Height;
        public TileType TileType;
    }
    public class GridModel
    {
        public GridData GridData { get; private set; }

        public GridModel(GridData gridData)
        {
            GridData = gridData;
        }
    }
}