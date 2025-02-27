using Raylib_cs;
using System.Numerics;
using UltimateTicTacToe.Game;
namespace UltimateTicTacToe.UI;

public class Board<TCell> where TCell : ICell<TCell>
{
    private Player.Token? Winner { get; set; }
    private Cell[] Cells { get; set; }
    private IEnumerable<int> Moves { get; set; }
    private Cell WinningPlayerCell { get; set; }
    private Transform2D Transform { get; }
    public bool InTransition
    {
        get
        {
            return WinningPlayerCell.InTransition || Cells.Any((x) => x.InTransition);
        }
    }
    public float TransitionValue
    {
        get
        {
            float max = WinningPlayerCell.TransitionValue;
            for (int i = 0; i < 9; i++)
            {
                max = Math.Max(Cells[i].TransitionValue, max);
            }
            return max;
        }
    }
    private TimeSpan TransitionTime { get; }
    public event Action<Board<TCell>, int>? Clicked;
    public Board(IBoard<TCell> board, Transform2D transform, TimeSpan transitionTime, IEnumerable<int>? moves = null)
    {
        Cells = new Cell[9];
        Winner = board.Winner;
        Transform = transform;
        Moves = moves ?? board.Moves;
        for (int i = 0; i < 9; i++)
        {
            var cell = board.Cells[i];
            var address = PositionOfIndex(i);
            var position = PixelPosition(transform, (int)address.X, (int)address.Y);
            var cellTransform = new Transform2D(position, 0, 1);
            Cells[i] = new Cell(cell, cellTransform, transitionTime);
        }
        foreach (var cell in Cells)
        {
            cell.Clicked += HandleClickedCell;
        }
        TransitionTime = transitionTime;
        var winningCellTransform = new Transform2D(transform.Position, 0, transform.Scale * 4);
        WinningPlayerCell = new Cell(board.WinningPlayerCell, winningCellTransform, transitionTime);
    }
    public void UpdateBoard(IBoard<TCell> board, IEnumerable<int>? moves = null)
    {
        Winner = board.Winner;
        Moves = moves ?? board.Moves;
        for (int i = 0; i < 9; i++)
        {
            var cell = board.Cells[i];
            Cells[i].UpdateCell(cell);
        }
        foreach (var cell in Cells)
        {
            cell.Clicked += HandleClickedCell;
        }
        WinningPlayerCell.UpdateCell(board.WinningPlayerCell);
    }
    public void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            Cells[i].Update();
        }
        bool gridCellInTransition = Cells.Any((x) => x.InTransition);
        if (gridCellInTransition == false)
            WinningPlayerCell.Update();
    }
    public void Draw()
    {
        bool gridCellInTransition = Cells.Any((x) => x.InTransition);
        if (Winner != null && gridCellInTransition == false)
        {
            WinningPlayerCell.Draw();
            return;
        }
        DrawGrid();
        for (int i = 0; i < 9; i++)
        {
            Cells[i].Draw();
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
    public void DrawPlaceableIndicator()
    {
        foreach (var cell in Cells)
        {
            cell.DrawPlaceableIndicator();
        }
    }
    public void HandleClickedCell(Cell cell)
    {
        var index = Array.IndexOf(Cells, cell);
        Clicked?.Invoke(this, index);
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