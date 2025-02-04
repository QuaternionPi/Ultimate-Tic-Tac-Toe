namespace UltimateTicTacToe.Game;
/*
The base object of Ultimate Tic Tac Toe
Cells store which player won them and weather or not they are placable
*/
//[JsonDerivedType(typeof(Game.Tile), 1)]
//[JsonDerivedType(typeof(Game.Grid<Game.Tile>), 2)]
//[JsonDerivedType(typeof(Game.Grid<Game.Grid<Game.Tile>>), 3)]
public interface ICell
{
    public Player.Token? Owner { get; }
    public bool Placeable { get; }
}
public interface ICell<TSelf> : ICell where TSelf : ICell<TSelf>
{
    public TSelf Place(Player.Token? player);
}