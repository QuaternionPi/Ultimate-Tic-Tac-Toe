using Raylib_cs;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToe;
public class Human : Player
{
    public Human(Symbol symbol, Color color, int score) : base(symbol, color, score)
    {
    }
    protected LargeGrid<Grid<Tile>, Tile>? Board;
    public override void BeginTurn(LargeGrid<Grid<Tile>, Tile> board, Player opponent)
    {
        Board = board;
        Board.Clicked += HandleClickedBoard;
    }
    public override void EndTurn()
    {
        if (Board == null)
        {
            return;
        }
        Board.Clicked -= HandleClickedBoard;
        Board = null;
    }
    public override void Update()
    {

    }
    private void HandleClickedBoard(ILargeBoard<Grid<Tile>, Tile> board, Grid<Tile> grid, Tile tile)
    {
        if (tile.Player != null || !((LargeGrid<Grid<Tile>, Tile>)board).Placeable[board.Location(tile).Item1])
        {
            return;
        }
        InvokePlayTurn(this, (LargeGrid<Grid<Tile>, Tile>)board, grid, tile);
    }
}