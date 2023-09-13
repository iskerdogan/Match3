using System;
using TMPro;
using UnityEngine;

namespace Game.View
{
    public interface ICellView
    {
        
    }
    public class CellView : MonoBehaviour,ICellView
    {
        public int Id { get; private set;}
        public TileView TileView { get; private set;}
        [SerializeField] private TextMeshPro coordinateText;
        [SerializeField] private TextMeshPro idText;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public void InitCell(int id,int width,int height)
        {
            Id = id;
            var cellPosition = new Vector3(width, height, 0);
            transform.position = cellPosition;
            name = "Cell " + "(" + width + " , " + height + ")";
            idText.SetText(id.ToString());
            coordinateText.SetText("(" + width + " ,  " + height + ")");
        }

        public void SetTileView(TileView tileView)
        {
            TileView = tileView;
        }
    }
}