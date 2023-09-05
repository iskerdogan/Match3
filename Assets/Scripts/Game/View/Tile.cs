using System;
using UnityEngine;

namespace Game.View
{
    public class Tile : MonoBehaviour
    {
        public TileType TileType
        {
            get => _tileType;
            set
            {
                spriteRenderer.color = value switch
                {
                    TileType.Red => Color.red,
                    TileType.Green => Color.green,
                    TileType.Blue => Color.blue,
                    TileType.Empty => Color.black,
                    _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
                };
                _tileType = value;
            }
        }
        private TileType _tileType;
        public SpriteRenderer spriteRenderer;

    }
}
