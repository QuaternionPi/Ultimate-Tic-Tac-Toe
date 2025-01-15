using Raylib_cs;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToe.Player;
public class Human : Player
{
    private UI.LargeBoard<Grid<Tile>, Tile>? BoardUI { get; set; }
    public Human(Symbol symbol, Color color, int score) : base(symbol, color, score)
    {
    }
    protected LargeGrid<Grid<Tile>, Tile>? Board;
    public override void BeginTurn(LargeGrid<Grid<Tile>, Tile> board, UI.LargeBoard<Grid<Tile>, Tile> boardUI, Player opponent)
    {
        Board = board;
        BoardUI = boardUI;
        BoardUI.Clicked += HandleClickedBoard;
    }
    public override void EndTurn()
    {
        if (BoardUI != null)
            BoardUI.Clicked -= HandleClickedBoard;
        Board = null;
        BoardUI = null;
    }
    public override void Update()
    {

    }
    private void HandleClickedBoard(UI.LargeBoard<Grid<Tile>, Tile> board, int index, int innerIndex)
    {
        if (Board == null)
        {
            return;
        }
        if (!Board.Placeable[index] && Board.Grids[innerIndex].AnyPlaceable)
        {
            return;
        }
        InvokePlayTurn(this, Board, index, innerIndex);
    }
}