// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using MyBox;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// namespace Game.View
// {
//     [CreateAssetMenu(menuName = "Create LevelConfig", fileName = "LevelConfig", order = 0)]
//     public class LevelConfig : ScriptableObject
//     {
//
//         public int width;
//         public int height;
//         public float distance;
//         public TileData[] tileDatas;
//         public TileSpawnerData[] tileSpawnerData;
//         public Dictionary<TileType, int> TileSpawnProbability;
//
//         [ButtonMethod()]
//         public async Task CreateTilesTest()
//         {
//             tileDatas = new TileData[width * height]; 
//             tileSpawnerData = new TileSpawnerData[width];
//             for (int x = 0; x < width; x++)
//             {
//                 for (int y = 0; y < height; y++)
//                 {
//                     TileData tileData = new TileData();
//                     var Id = width * y + x;
//                     tileData.Id = Id;
//                     tileData.Coordinate = new Vector2Int(x, y);
//                     tileDatas[Id] = tileData;
//                 }
//             }
//
//             await Task.Delay(10);
//             SetNames();
//         }
//         
//         public async Task SetNames()
//         {
//             for (int i = 0; i < tileSpawnerData.Length; i++)
//             {
//                 tileSpawnerData[i].name = i.ToString();
//                 tileSpawnerData[i].spawnTileDatas = new SpawnTileData[Enum.GetNames(typeof(TileType)).Length-1];
//             }
//             await Task.Delay(10);
//             CreateSpawnerTest();
//         }
//         
//         public void CreateSpawnerTest()
//         {
//             for (int i = 0; i < tileSpawnerData.Length; i++)
//             {
//                 for (int j = 0; j < Enum.GetNames(typeof(TileType)).Length-1; j++)
//                 {
//                     tileSpawnerData[i].spawnTileDatas[j].name = ((TileType) j) + " Probability";
//                     tileSpawnerData[i].spawnTileDatas[j].type = (TileType) j;
//                 }
//             }
//         }
//
//         [ButtonMethod()]
//         public void Clear()
//         {
//             tileDatas = null; 
//             tileSpawnerData = null;
//         }
//     }
//
//
//     
//
//     [Serializable]
//     public class TileData
//     {
//         public int Id;
//         public Vector2Int Coordinate;
//         public TileType TileType;
//     }
//     
//     [Serializable]
//     public class TileSpawnerData
//     {
//         [HideInInspector] public string name;
//         public SpawnTileData[] spawnTileDatas;
//         
//         public TileSpawnerData(string name,SpawnTileData[] spawnTileDatas)
//         {
//             this.name = name;
//             this.spawnTileDatas = spawnTileDatas;
//         }
//     }
//     
//     [Serializable]
//     public class SpawnTileData
//     {
//         [HideInInspector] public string name;
//         
//         public TileType type;
//         public int spawnProbability;
//         
//         public SpawnTileData(TileType type,int spawnProbability)
//         {
//             this.type = type;
//             this.spawnProbability = spawnProbability;
//         }
//     }
//
//     public enum TileType
//     {
//         Empty,
//         Red,
//         Green,
//         Blue
//     }
// }