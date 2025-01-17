using System.Numerics;
using Raylib_cs;
namespace UltimateTicTacToe.UI;

public class LargeBoard<TGrid, TCell> where TCell : Game.ICell where TGrid : Game.IBoard<TCell>
{
    private Game.Player? Player { get; set; }
    private Board<TCell>[] Boards { get; set; }
    private IEnumerable<(int, int)> Moves { get; set; }
    private Cell WinningPlayerCell { get; set; }
    private Transform2D Transform { get; }
    public bool InTransition
    {
        get
        {
            return WinningPlayerCell.InTransition || Boards.Any((x) => x.InTransition);
        }
    }
    public float TransitionValue
    {
        get
        {
            float max = WinningPlayerCell.TransitionValue;
            for (int i = 0; i < 9; i++)
            {
                max = Math.Max(max, Boards[i].TransitionValue);
            }
            return max;
        }
    }
    public event Action<LargeBoard<TGrid, TCell>, int, int>? Clicked;
    public LargeBoard(Game.ILargeBoard<TGrid, TCell> largeBoard, Transform2D transform)
    {
        Boards = new Board<TCell>[9];
        Player = largeBoard.Player;
        Transform = transform;
        Moves = largeBoard.PlayableIndices;
        for (int i = 0; i < 9; i++)
        {
            var board = largeBoard.Grids[i];
            var address = PositionOfIndex(i);
            var position = PixelPosition(transform, (int)address.X, (int)address.Y);
            var cellTransform = new Transform2D(position, 0, 1);
            var moves = from move in Moves where move.Item1 == i select move.Item2;
            Boards[i] = new Board<TCell>(board, cellTransform);
        }
        foreach (var cell in Boards)
        {
            cell.Clicked += HandleClickedCell;
        }
        WinningPlayerCell = new Cell(largeBoard.WinningPlayerCell, new Transform2D(transform.Position, 1, transform.Scale * 4));
    }
    public void UpdateLargeBoard(Game.ILargeBoard<TGrid, TCell> largeBoard)
    {
        Player = largeBoard.Player;
        for (int i = 0; i < 9; i++)
        {
            var moves = from move in Moves where move.Item1 == i select move.Item2;
            var board = largeBoard.Grids[i];
            Boards[i].UpdateBoard(board, moves);
        }
        foreach (var cell in Boards)
        {
            cell.Clicked += HandleClickedCell;
        }
        Moves = largeBoard.PlayableIndices;
        WinningPlayerCell.UpdateCell(largeBoard.WinningPlayerCell);
    }
    public void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            Boards[i].Update();
        }
        bool gridCellInTransition = Boards.Any((x) => x.InTransition);
        if (gridCellInTransition == false)
            WinningPlayerCell.Update();
    }
    public void Draw()
    {
        bool gridCellInTransition = Boards.Any((x) => x.InTransition);
        if (Player != null && gridCellInTransition == false)
        {
            WinningPlayerCell.Draw();
            return;
        }
        DrawGrid();
        for (int i = 0; i < 9; i++)
        {
            Boards[i].Draw();
        }
        foreach ((int i, int j) in Moves)
        {
            Boards[i].DrawPlaceableIndicator();
        }
    }
    public void DrawGrid()
    {
        Transform2D transform = Transform;
        int lineGap = (int)(50 * transform.Scale);
        int lineLength = (int)(130 * transform.Scale);
        int lineWidth = (int)(2 * transform.Scale);
        int x = (int)transform.Position.X;
        int y = (int)transform.Position.Y;
        Color color = Color.LIGHTGRAY;

        Graphics.Draw.Rectangle(x - lineWidth / 2 + lineGap / 2, y - lineLength / 2, lineWidth, lineLength, color);
        Graphics.Draw.Rectangle(x - lineWidth / 2 - lineGap / 2, y - lineLength / 2, lineWidth, lineLength, color);
        Graphics.Draw.Rectangle(x - lineLength / 2, y - lineWidth / 2 + lineGap / 2, lineLength, lineWidth, color);
        Graphics.Draw.Rectangle(x - lineLength / 2, y - lineWidth / 2 - lineGap / 2, lineLength, lineWidth, color);
    }
    public void HandleClickedCell(Board<TCell> board, int innerIndex)
    {
        var index = Array.IndexOf(Boards, board);
        Clicked?.Invoke(this, index, innerIndex);
    }
    public static Vector2 PixelPosition(Transform2D transform, int i, int j)
    {
        int x = (int)(transform.Position.X + (i - 1) * 50 * transform.Scale);
        int y = (int)(transform.Position.Y + (j - 1) * 50 * transform.Scale);
        return new Vector2(x, y);
    }
    public static Vector2 PositionOfIndex(int index)
    {
        int x = index / 3;
        int y = index % 3;
        return new Vector2(x, y);
    }
}