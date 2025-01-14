using System.Numerics;
using Raylib_cs;
namespace UltimateTicTacToe.UI;

public class LargeBoard<TGrid, TCell> where TCell : Game.ICell where TGrid : Game.IBoard<TCell>
{
    private Player.Player? Player { get; }
    private Board<TCell>[] Boards { get; }
    private Cell WinningPlayerCell { get; }
    public Transform2D Transform { get; }
    public bool InTransition
    {
        get
        {
            if (WinningPlayerCell.InTransition)
            {
                return true;
            }
            for (int i = 0; i < 9; i++)
            {
                if (Boards[i].InTransition)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public float TransitionValue
    {
        get
        {
            var values = new float[9];
            for (int i = 0; i < 9; i++)
            {
                values[i] = Boards[i].TransitionValue;
            }
            float max = values[0];
            foreach (var value in values)
            {
                max = Math.Max(value, max);
            }
            max = Math.Max(max, WinningPlayerCell.TransitionValue);
            return max;
        }
    }
    public event Action<LargeBoard<TGrid, TCell>, int, int>? Clicked;
    public LargeBoard(Game.ILargeBoard<TGrid, TCell> largeBoard, Transform2D transform)
    {
        Boards = new Board<TCell>[9];
        Player = largeBoard.Player;
        Transform = transform;
        for (int i = 0; i < 9; i++)
        {
            var board = largeBoard.Cells[i];
            var address = PositionOfIndex(i);
            var position = PixelPosition(transform, (int)address.X, (int)address.Y);
            var cellTransform = new Transform2D(position, 0, 1);
            Boards[i] = new Board<TCell>(board, cellTransform);
        }
        foreach (var cell in Boards)
        {
            cell.Clicked += HandleClickedCell;
        }
        WinningPlayerCell = new Cell(largeBoard.WinningPlayerCell, new Transform2D(transform.Position, 1, transform.Scale * 4));
    }
    public void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            Boards[i].Update();
        }
        bool gridCellInTransition = false;
        for (int i = 0; i < 9; i++)
        {
            gridCellInTransition |= Boards[i].InTransition;
        }
        if (gridCellInTransition == false)
            WinningPlayerCell.Update();
    }
    public void Draw()
    {
        bool gridCellInTransition = false;
        for (int i = 0; i < 9; i++)
        {
            gridCellInTransition |= Boards[i].InTransition;
        }
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