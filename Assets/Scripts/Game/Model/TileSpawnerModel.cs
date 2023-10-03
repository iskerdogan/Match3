namespace Game.Model
{
    public interface ITileSpawnerModel
    {
        void SetData(string[] data);
        int RedProbability { get;}
        int GreenProbability { get;}
        int BlueProbability { get;}
    }
    public class TileSpawnerModel:ITileSpawnerModel
    {
        public int RedProbability { get; private set; }
        public int GreenProbability { get; private set; }
        public int BlueProbability { get; private set; }
        
        public void SetData(string[] data)
        {
            RedProbability = int.Parse(data[0]);
            GreenProbability = int.Parse(data[1]);
            BlueProbability = int.Parse(data[2]);
        }
    }
}