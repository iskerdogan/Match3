namespace Game.Model
{
    public interface ITileModel
    {
        void SetData(int tileType);
    }
    public class TileModel : ITileModel
    {
        public int TileType { get; private set; }
        
        public void SetData(int tileType)
        {
            TileType = tileType;
        }
    }
}