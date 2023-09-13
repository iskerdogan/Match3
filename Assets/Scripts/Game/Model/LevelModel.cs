namespace Game.Model
{
    public interface ILevelModel
    {
        void SetData();
    }
    
    public class LevelModel : ILevelModel
    {
        public int LevelId { get; private set; } = 1;
        
        public void SetData()
        {
            
        }
    }
}