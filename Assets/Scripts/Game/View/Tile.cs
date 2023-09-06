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
        private Transform _destinationTransform;
        private float speed;
        private void Update()
        {
            if (_destinationTransform == null) return;
            Move();
        }

        public void MoveTile(Transform destinationTransform)
        {
            transform.SetParent(null);
            _destinationTransform = destinationTransform;
        }

        private void Move()
        {
            speed = Time.deltaTime * 5;
            var distance = Vector3.Distance(transform.position, _destinationTransform.position);
            speed *= 1 / distance;
            transform.position = Vector3.Lerp(transform.position, _destinationTransform.position, speed);
            if (distance < 0.1f)
            {
                transform.SetParent(_destinationTransform);
                _destinationTransform = null;
            }
        }
    }
}
