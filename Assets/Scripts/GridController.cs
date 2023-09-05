// using System;
// using System.Collections.Generic;
// using Game.View;
// using MyBox;
// using Unity.Mathematics;
// using UnityEngine;
//
// public enum Direction
// {
//     Up,
//     Down,
//     Left,
//     Right
// }
//
// public class GridController : MonoBehaviour
// {
//     public static GridController Instance;
//     public Cell prefab;
//     public Cell[] cells;
//     [SerializeField] private LevelConfig levelConfig;
//     private GameObject tileParent;
//
//     private void Awake()
//     {
//         Instance = this;
//     }
//
//     private void Start()
//     {
//         // Subscribe();
//     }
//
//     private void OnDestroy()
//     {
//         // Unsubscribe();
//     }
//
//     [ButtonMethod()]
//     public void CreateGird()
//     {
//         ClearTilesTest();
//         var tilePosition = Vector3.zero;
//         tileParent = new GameObject("TileParent");
//         foreach (var tileData in levelConfig.tileDatas)
//         {
//             Cell cell = Instantiate(prefab, Vector3.zero, quaternion.identity, tileParent.transform);
//             // cell.Id = tileData.id;
//             // cell.Coordinate = tileData.coordinate;
//             // cell.TileType = tileData.tileType;
//             tilePosition = new Vector3(cell.Coordinate.x, cell.Coordinate.y, 0);
//             cell.transform.position = tilePosition;
//             cell.name = "Tile " + "(" + cell.Coordinate.x + " , " + cell.Coordinate.y + ")";
//             // cells.Add(cell);
//         }
//
//         SetNeighbour();
//     }
//
//     private void SetNeighbour()
//     {
//         foreach (var cell in cells)
//         {
//             // cell.neighbourUp = GetNeighbour(Direction.Up, cell);
//             // cell.neighbourDown = GetNeighbour(Direction.Down, cell);
//             // cell.neighbourRight = GetNeighbour(Direction.Right, cell);
//             // cell.neighbourLeft = GetNeighbour(Direction.Left,cell);
//         }
//     }
//
//     // private Cell GetNeighbour(Direction neighbour,Cell cell)
//     // {
//     //     
//     //     // Tile result = null;
//     //     // switch (neighbour)
//     //     // {
//     //     //     case Direction.Up:
//     //     //         if (tile.Coordinate.y == levelConfig.height - 1) return null;
//     //     //         result = tiles.Find(x => x.Id == tile.Id + levelConfig.width);
//     //     //         break;
//     //     //     case Direction.Down:
//     //     //         if (tile.Coordinate.y == 0) return null;
//     //     //         result = tiles.Find(x => x.Id == tile.Id - levelConfig.width);
//     //     //         break;
//     //     //     case Direction.Left:
//     //     //         if (tile.Coordinate.x == 0) return null;
//     //     //         result = tiles.Find(x => x.Id == tile.Id - 1);
//     //     //         break;
//     //     //     case Direction.Right:
//     //     //         if (tile.Coordinate.x == levelConfig.width - 1) return null;
//     //     //         result = tiles.Find(x => x.Id == tile.Id + 1);
//     //     //         break;
//     //     //     default:
//     //     //         throw new ArgumentOutOfRangeException(nameof(neighbour), neighbour, null);
//     //     // }
//     //     // return result;
//     // }
//
//     public void ClearTilesTest()
//     {
//         cells = null;
//         // if (!tileParent) return;
//         // Destroy(tileParent);
//     }
//
//     // private void Subscribe()
//     // {
//     //     InputManager.Clicked += Clicked;
//     // }
//     //
//     // private void Unsubscribe()
//     // {
//     //     InputManager.Clicked -= Clicked;
//     // }
//
//     public List<Cell> tileMatch;
//     private void CheckTileType(Cell cell) {
//         if (cell.CheckNeighbourUp() && !tileMatch.Contains(cell.neighbourUp))
//         {
//             tileMatch.Add(cell.neighbourUp);
//             CheckTileType(cell.neighbourUp);
//         }
//         
//         if (cell.CheckNeighbourDown()&& !tileMatch.Contains(cell.neighbourDown))
//         {
//             tileMatch.Add(cell.neighbourDown);
//             CheckTileType(cell.neighbourDown);
//         }
//         
//         if (cell.CheckNeighbourleft()&& !tileMatch.Contains(cell.neighbourLeft))
//         {
//             tileMatch.Add(cell.neighbourLeft);
//             CheckTileType(cell.neighbourLeft);
//         }
//         
//         if (cell.CheckNeighbourRigth()&& !tileMatch.Contains(cell.neighbourRight))
//         {
//             tileMatch.Add(cell.neighbourRight);
//             CheckTileType(cell.neighbourRight);
//         }
//     }
//     
//     private void Clicked()
//     {
//         if (Camera.main == null) return;
//         var rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
//         RaycastHit hit;
//         if (!Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit)) return;
//         if (!hit.transform.TryGetComponent(out Cell tile)) return;
//         // if (tile.TileType == TileType.empty) return;
//         tileMatch.Clear();
//         CheckTileType(tile);
//         Debug.Log(tileMatch.Count);
//     }
// }