using System;
using MyBox;
using UnityEngine;

namespace Game.View
{
    [CreateAssetMenu(menuName = "Create LevelConfig", fileName = "LevelConfig", order = 0)]
    public class LevelConfig : ScriptableObject
    {

        public int width;
        public int height;
        public float distance;
        public TileData[] tileDatas;
    

        [ButtonMethod()]
        public void CreateTilesTest()
        {
            tileDatas = new TileData[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    TileData tileData = new TileData();
                    var Id = width * y + x;
                    tileData.Id = Id;
                    tileData.Coordinate = new Vector2Int(x, y);
                    tileDatas[Id] = tileData;
                }
            }
        }
    }

    [Serializable]
    public class TileData
    {
        public int Id;
        public Vector2Int Coordinate;
        public TileType TileType;
    }

    public enum TileType
    {
        Red,
        Green,
        Blue,
        Empty
    }
}