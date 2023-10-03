namespace Game.Model
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    public class CellModel 
    {
        private IGridModel _gridModel;
        public CellModel(IGridModel gridModel)
        {
            _gridModel = gridModel;
        }
        
        public int Id { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public TileModel TileModel { get; private set; }
        
        public void SetData(int id,int x,int y)
        {
            Id = id;
            X = x;
            Y = y;
        }
        
        public void SetTileModel(TileModel tileModel)
        {
            TileModel = tileModel;
        }
        
        public TileModel GetTileModel()
        {
            return TileModel;
        }
        
        public bool CheckNeighbourMatch(Direction direction)
        {
            var neighbourCellModel = GetNeighbour(direction);
            return CheckNeighbourTypeMatch(neighbourCellModel);
        }

        public CellModel GetNeighbour(Direction direction)
        {
            var neighbourPoint = GetNeighbourPoint(direction);
            return _gridModel.GetCellModel(neighbourPoint.Item1, neighbourPoint.Item2);
        }

        private bool CheckNeighbourTypeMatch(CellModel neighbourCellModel)
        {
            var isCellNull = neighbourCellModel == null;
            var isTileNull = neighbourCellModel?.TileModel == null;
            var isMatch = TileModel.TileType == neighbourCellModel?.TileModel?.TileType;
            return !isCellNull && !isTileNull && isMatch;
        }

        private (int,int) GetNeighbourPoint(Direction direction)
        {
            return direction switch
            {
               Direction.Up => (X,Y+1),
                Direction.Down => (X,Y-1),
                Direction.Left => (X-1,Y),
                Direction.Right => (X+1,Y),
            };
        }
    }
}