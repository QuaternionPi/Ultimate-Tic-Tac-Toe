using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe.Game;
public class Human : Player
{
    [JsonIgnore]
    private UI.LargeBoard<Grid<Tile>, Tile>? BoardUI { get; set; }
    [JsonIgnore]
    private bool MoveMade { get; set; }
    [JsonConstructor]
    public Human(Symbol shape, Color color, int score) : base(shape, color, score)
    {
        MoveMade = false;
    }
    [JsonIgnore]
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
    private void HandleClickedBoard(UI.LargeBoard<Grid<Tile>, Tile> board, (int, int) move)
    {
        if (Board == null)
        {
            return;
        }
        var index = move.Item1;
        bool placeable = Board.Placeable[index] && Board.Grids[index].AnyPlaceable;
        if (!placeable || MoveMade)
        {
            return;
        }
        MoveMade = true;
        InvokePlayTurn(this, Board, move);
    }
}