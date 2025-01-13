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
    public Player.Player? Player { get; }
    public ICell Place(Player.Player? player);
}