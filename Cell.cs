using System.Text.Json.Serialization;

namespace UltimateTicTacToe
{
    /*
    The base object of Ultimate Tic Tac Toe
    Cells store which player won them and weather or not they are placable
    */
    [JsonDerivedType(typeof(Game.Tile), 1)]
    [JsonDerivedType(typeof(Game.Grid<Game.Tile>), 2)]
    [JsonDerivedType(typeof(Game.Grid<Game.Grid<Game.Tile>>), 3)]
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