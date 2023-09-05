using System;
using TMPro;
using UnityEngine;

namespace Game.View
{
    public class Cell : MonoBehaviour
    {
        public int Id
        {
            get => _id;
            set
            {
                idText.SetText(value.ToString());
                _id = value;
            }
        }    
    
        public int Width
        {
            get => width;
            set
            {
                coordinateText.SetText("(" + value + " , " + height + ")");
                width = value;
            }
        }        
        
        public int Height
        {
            get => height;
            set
            {
                coordinateText.SetText("(" + width + " , " + value + ")");
                height = value;
            }
        }
        public Tile CurrentTile
        {
            get => currentTile;
            set => currentTile = value;
        }

        private int _id;
        private int width;
        private int height;
        private Tile currentTile;
        public TextMeshPro coordinateText;
        public TextMeshPro idText;
        public SpriteRenderer spriteRenderer;

        public Cell neighbourUp;
        public Cell neighbourDown;
        public Cell neighbourLeft;
        public Cell neighbourRight;

        public bool CheckNeighbourUp()
        {
            if (!neighbourUp) return false;
            if (!neighbourUp.currentTile) return false;
            if (neighbourUp.currentTile.TileType == TileType.Empty) return false;
            return currentTile.TileType == neighbourUp.currentTile.TileType;
        }
    
        public bool CheckNeighbourDown()
        {
            if (!neighbourDown) return false;
            if (!neighbourDown.currentTile) return false;
            if (neighbourDown.currentTile.TileType == TileType.Empty) return false;
            return currentTile.TileType == neighbourDown.currentTile.TileType;
        }
    
        public bool CheckNeighbourleft()
        {
            if (!neighbourLeft) return false;
            if (!neighbourLeft.currentTile) return false;
            if (neighbourLeft.currentTile.TileType == TileType.Empty) return false;
            return currentTile.TileType == neighbourLeft.currentTile.TileType;
        }
    
        public bool CheckNeighbourRigth()
        {
            if (!neighbourRight) return false;
            if (!neighbourRight.currentTile) return false;
            if (neighbourRight.currentTile.TileType == TileType.Empty) return false;
            return currentTile.TileType == neighbourRight.currentTile.TileType;
        }
    }
}