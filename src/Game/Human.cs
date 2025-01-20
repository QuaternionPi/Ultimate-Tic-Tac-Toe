using Raylib_cs;

namespace UltimateTicTacToe.Game;
public class Human : Player
{
    private UI.LargeBoard<Grid<Tile>, Tile>? BoardUI { get; set; }
    private bool MoveMade { get; set; }
    public Human(Symbol symbol, Color color, int score) : base(symbol, color, score)
    {
        MoveMade = false;
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
        MoveMade = false;
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
        bool placeable = Board.Placeable[index] && Board.Grids[index].AnyPlaceable;
        if (!placeable || MoveMade)
        {
            return;
        }
        MoveMade = true;
        InvokePlayTurn(this, Board, index, innerIndex);
    }
}