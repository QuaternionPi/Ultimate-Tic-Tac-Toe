namespace UltimateTicTacToe
{
    public interface ICell
    {
        public Player? Player { get; }
        public bool Placeable { get; }
        public ICell Create(Player? player, Transform2D transform, bool placeable);
        public ICell Place(IEnumerable<ICell> cells, Player player, bool placeable);
        public ICell DeepCopyPlacable(bool placeable);
        public List<Address> PathTo(ICell cell);
        public bool Contains(ICell cell);
    }
    public interface IClickableCell : ICell
    {
        Transform2D Transform { get; }
        public delegate void ClickHandler(IEnumerable<ICell> cells);
        public event ClickHandler? Clicked;
    }
}