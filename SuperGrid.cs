using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

class SuperGrid : IDrawable, ITransform
{
    public SuperGrid(Vector2 position)
    {
        Position = position;
        Rotation = 0;
        Scale = 1;
        _validGrids = new List<Vector2>();
        _grids = new Grid[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int x = 200 * (i - 1) + (int)Position.X;
                int y = 200 * (j - 1) + (int)Position.Y;
                Grid grid = new Grid(new Vector2(x, y));
                _grids[i, j] = grid;
                grid.Clicked += HandleClickedGrid;
                _validGrids.Add(new Vector2(i, j));
            }
        }
    }
    public List<Vector2> ValidPositions()
    {
        List<Vector2> validPositions = new List<Vector2>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_grids[i, j].Solved)
                {
                    validPositions.Add(new Vector2(i, j));
                }
            }
        }
        return validPositions;
    }
    public bool IsValidPlacement(Vector2 position, Vector2 subPosition)
    {
        if (Solved)
        {
            return false;
        }
        int x = (int)position.X;
        int y = (int)position.Y;
        if (x > 2 | x < 0 | y > 2 | y < 0)
        {
            throw new IndexOutOfRangeException("Position not in grid");
        }
        if (_validGrids.Contains(position))
        {
            Grid grid = _grids[x, y];
            if (grid.IsValidPlacement(subPosition))
            {
                return true;
            }
            return false;
        }
        return false;
    }
    public void PlaceTile(Tile tile, Vector2 position, Vector2 subPosition)
    {
        if (IsValidPlacement(position, subPosition) == false)
        {
            throw new Exception("Cannot place tile");
        }
        int x = (int)position.X;
        int y = (int)position.Y;
        Grid grid = _grids[x, y];
        grid.PlaceTile(tile, subPosition);

        _validGrids.Clear();
        int i = (int)subPosition.X;
        int j = (int)subPosition.Y;
        if (_grids[i, j].ValidPositions().Count() > 0)
        {
            _validGrids.Add(new Vector2(i, j));
        }
        else
        {
            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    _validGrids.Add(new Vector2(i, j));
                }
            }
        }

        TestForShape();
    }
    public void TestForShape()
    {
        if (Shape != Tile.TileShape.DEFAULT)
        {
            return;
        }
        Tile.TileShape test(Tile.TileShape shape1, Tile.TileShape shape2, Tile.TileShape shape3)
        {
            if (shape1 == shape2 && shape1 == shape3)
            {
                return shape1;
            }
            return Tile.TileShape.DEFAULT;
        }
        Tile.TileShape testRows()
        {
            Tile.TileShape result;
            result = test(_grids[0, 0].Shape, _grids[1, 0].Shape, _grids[2, 0].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            result = test(_grids[0, 1].Shape, _grids[1, 1].Shape, _grids[2, 1].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            result = test(_grids[0, 2].Shape, _grids[1, 2].Shape, _grids[2, 2].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            return Tile.TileShape.DEFAULT;
        }
        Tile.TileShape testColumns()
        {
            Tile.TileShape result;
            result = test(_grids[0, 0].Shape, _grids[0, 1].Shape, _grids[0, 2].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            result = test(_grids[1, 0].Shape, _grids[1, 1].Shape, _grids[1, 2].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            result = test(_grids[2, 0].Shape, _grids[2, 1].Shape, _grids[2, 2].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            return Tile.TileShape.DEFAULT;
        }
        Tile.TileShape testDiagonals()
        {
            Tile.TileShape result;
            result = test(_grids[0, 0].Shape, _grids[1, 1].Shape, _grids[2, 2].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            result = test(_grids[0, 2].Shape, _grids[1, 1].Shape, _grids[2, 0].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            return Tile.TileShape.DEFAULT;
        }
        Tile.TileShape testAll()
        {
            Tile.TileShape result;
            result = testRows();
            if (result != Tile.TileShape.DEFAULT) { return result; }

            result = testColumns();
            if (result != Tile.TileShape.DEFAULT) { return result; }

            result = testDiagonals();
            if (result != Tile.TileShape.DEFAULT) { return result; }
            return Tile.TileShape.DEFAULT;
        }
        Shape = testAll();
    }
    public Vector2 GridPosition(Grid grid)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_grids[i, j] == grid)
                {
                    return new Vector2(i, j);
                }
            }
        }
        throw new ArgumentException("Tile not found");
    }
    public void HandleClickedGrid(object? sender, Grid.ClickedEventArgs args)
    {
        if (sender == null)
        {
            return;
        }
        Tile tile = args._tile;
        Grid grid = (Grid)sender;
        ClickedEventArgs newArgs = new ClickedEventArgs(tile, grid);
        Clicked?.Invoke(this, newArgs);
    }
    public void SlideDown(float deltaY)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _grids[i, j].SlideDown(deltaY);
            }
        }
        Position = new Vector2(Position.X, Position.Y - deltaY);
    }
    public void DrawPossibilities()
    {
        foreach (Vector2 position in _validGrids)
        {
            int i = (int)position.X;
            int j = (int)position.Y;
            _grids[i, j].DrawPossibilities();
        }
    }
    public void Draw()
    {
        int lineGap = 200;
        int lineLength = 550;
        int lineWidth = 8;
        DrawRectangle((int)Position.X - lineWidth / 2 + lineGap / 2, (int)Position.Y - lineLength / 2, lineWidth, lineLength, Color.LIGHTGRAY);
        DrawRectangle((int)Position.X - lineWidth / 2 - lineGap / 2, (int)Position.Y - lineLength / 2, lineWidth, lineLength, Color.LIGHTGRAY);
        DrawRectangle((int)Position.X - lineLength / 2, (int)Position.Y - lineWidth / 2 + lineGap / 2, lineLength, lineWidth, Color.LIGHTGRAY);
        DrawRectangle((int)Position.X - lineLength / 2, (int)Position.Y - lineWidth / 2 - lineGap / 2, lineLength, lineWidth, Color.LIGHTGRAY);

        foreach (Grid grid in _grids)
        {
            grid.Draw();
        }
    }
    public void Update()
    {
        foreach (Grid grid in _grids)
        {
            grid.Update();
        }
    }
    public bool Solved
    {
        get { return Shape != Tile.TileShape.DEFAULT; }
    }
    public Tile.TileShape Shape
    {
        get; protected set;
    }
    public Vector2 Position
    {
        get; protected set;
    }
    public float Rotation
    {
        get; protected set;
    }
    public float Scale
    {
        get; protected set;
    }
    public event EventHandler<ClickedEventArgs>? Clicked;
    public class ClickedEventArgs
    {
        public ClickedEventArgs(Tile tile, Grid grid)
        {
            _tile = tile;
            _grid = grid;
        }
        public Tile _tile;
        public Grid _grid;
    }
    private Grid[,] _grids;
    private List<Vector2> _validGrids;
}