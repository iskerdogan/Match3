using UnityEngine;

namespace Game.View
{
    public enum TileType
    {
        Empty,
        Red,
        Green,
        Blue
    }
    
    public class TileView : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        private Transform _destinationTransform;
        private float _speed;
        
        public void InitTile(int width,int height,TileType tileType)
        {
            var tilePosition = new Vector3(width, height, 0);
            transform.position = tilePosition;
            name = "Tile " + "(" +tileType+ ")";
            SetTile(tileType);
        }

        private void SetTile(TileType tileType)
        {
            spriteRenderer.color = tileType switch
            {
                TileType.Empty => Color.black,
                TileType.Red => Color.red,
                TileType.Green => Color.green,
                TileType.Blue => Color.blue,
                _ => Color.grey,
            };
        }
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
            _speed = Time.deltaTime * 20;
            var position = transform.position;
            var destinationPosition = _destinationTransform.position;
            var distance = Vector3.Distance(position, destinationPosition);
            _speed *= 1 / distance;
            position = Vector3.Lerp(position, destinationPosition, _speed);
            transform.position = position;
            if (distance < 0.1f)
            {
                transform.SetParent(_destinationTransform);
                _destinationTransform = null;
            }
        }
    }
}
