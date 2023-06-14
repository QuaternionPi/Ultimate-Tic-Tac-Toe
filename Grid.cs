using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

class Grid : GameObject, ITransform
{
    public Grid(Vector2 position)
    {
        Position = position;
        Rotation = 0;
        Scale = 1;
        _tiles = new Tile[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Tile tile = Tile.defaultTile();
                tile.Position = PixelPosition(new Vector2(i, j));
                _tiles[i, j] = tile;
                tile.Clicked += HandleClickedTile;
            }
        }
    }
    public List<Vector2> ValidPositions()
    {
        if (Solved)
        {
            return new List<Vector2>();
        }
        List<Vector2> validPositions = new List<Vector2>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_tiles[i, j].Shape == Tile.TileShape.DEFAULT)
                {
                    validPositions.Add(new Vector2(i, j));
                }
            }
        }
        return validPositions;
    }
    public bool IsValidPlacement(Vector2 position)
    {
        if (Solved)
        {
            return false;
        }

        int i = (int)position.X;
        int j = (int)position.Y;
        if (i > 2 | i < 0 | j > 2 | j < 0)
        {
            return false;
        }

        if (_tiles[i, j].Shape != Tile.TileShape.DEFAULT)
        {
            return false;
        }
        return true;
    }
    public void PlaceTile(Tile tile, Vector2 position)
    {
        if (IsValidPlacement(position) == false)
        {
            throw new Exception("Cannot place tile");
        }
        int i = (int)position.X;
        int j = (int)position.Y;
        tile.Position = PixelPosition(new Vector2(i, j));
        _tiles[i, j].Destroy();
        _tiles[i, j] = tile;

        TestForShape();
    }
    public Vector2 PixelPosition(Vector2 positionInGrid)
    {
        int i = (int)positionInGrid.X;
        int j = (int)positionInGrid.Y;
        if (i > 2 | i < 0 | j > 2 | j < 0)
        {
            throw new IndexOutOfRangeException("Position not in grid");
        }
        int x = (int)Position.X + (i - 1) * 50;
        int y = (int)Position.Y + (j - 1) * 50;
        return new Vector2(x, y);
    }
    public Vector2 GridPosition(Tile tile)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_tiles[i, j] == tile)
                {
                    return new Vector2(i, j);
                }
            }
        }
        throw new ArgumentException("Tile not found");
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
            result = test(_tiles[0, 0].Shape, _tiles[1, 0].Shape, _tiles[2, 0].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            result = test(_tiles[0, 1].Shape, _tiles[1, 1].Shape, _tiles[2, 1].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            result = test(_tiles[0, 2].Shape, _tiles[1, 2].Shape, _tiles[2, 2].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            return Tile.TileShape.DEFAULT;
        }
        Tile.TileShape testColumns()
        {
            Tile.TileShape result;
            result = test(_tiles[0, 0].Shape, _tiles[0, 1].Shape, _tiles[0, 2].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            result = test(_tiles[1, 0].Shape, _tiles[1, 1].Shape, _tiles[1, 2].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            result = test(_tiles[2, 0].Shape, _tiles[2, 1].Shape, _tiles[2, 2].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            return Tile.TileShape.DEFAULT;
        }
        Tile.TileShape testDiagonals()
        {
            Tile.TileShape result;
            result = test(_tiles[0, 0].Shape, _tiles[1, 1].Shape, _tiles[2, 2].Shape);
            if (result != Tile.TileShape.DEFAULT) { return result; }

            result = test(_tiles[0, 2].Shape, _tiles[1, 1].Shape, _tiles[2, 0].Shape);
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

        Tile.TileShape result = testAll();
        Shape = result;
        if (Shape == Tile.TileShape.X)
        {
            _victoryTile = Tile.xTile();
            _victoryTile.Position = Position;
            _victoryTile.Scale *= 5;
        }
        if (Shape == Tile.TileShape.O)
        {
            _victoryTile = Tile.oTile();
            _victoryTile.Position = Position;
            _victoryTile.Scale *= 5;
        }
        if (Solved)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _tiles[i, j].Destroy();
                }
            }
        }
    }
    public void HandleClickedTile(object? sender, EventArgs eventArgs)
    {
        if (sender == null)
        {
            return;
        }
        ClickedEventArgs args = new ClickedEventArgs((Tile)sender);
        Clicked?.Invoke(this, args);
    }
    public void SlideDown(float deltaY)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _tiles[i, j].SlideDown(deltaY);
            }
        }
        _victoryTile?.SlideDown(deltaY);
        Position = new Vector2(Position.X, Position.Y - deltaY);
    }
    public void DrawPossibilities()
    {
        if (Solved)
        {
            return;
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Tile tile = _tiles[i, j];
                if (tile.Shape != Tile.TileShape.DEFAULT)
                {
                    continue;
                }
                Vector2 position = tile.Position;
                int width = 20;
                DrawRectangle((int)position.X - width / 2, (int)position.Y - width / 2, width, width, Color.LIGHTGRAY);
            }
        }
    }
    public override void Draw()
    {
        int lineGap = 50;
        int lineLength = 150;
        int lineWidth = 4;
        DrawRectangle((int)Position.X - lineWidth / 2 + lineGap / 2, (int)Position.Y - lineLength / 2, lineWidth, lineLength, Color.LIGHTGRAY);
        DrawRectangle((int)Position.X - lineWidth / 2 - lineGap / 2, (int)Position.Y - lineLength / 2, lineWidth, lineLength, Color.LIGHTGRAY);
        DrawRectangle((int)Position.X - lineLength / 2, (int)Position.Y - lineWidth / 2 + lineGap / 2, lineLength, lineWidth, Color.LIGHTGRAY);
        DrawRectangle((int)Position.X - lineLength / 2, (int)Position.Y - lineWidth / 2 - lineGap / 2, lineLength, lineWidth, Color.LIGHTGRAY);
    }
    public override void OnDestroy()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _tiles[i, j].Destroy();
            }
        }
        if (_victoryTile != null)
        {
            _victoryTile.Destroy();
        }
    }
    public Tile.TileShape Shape
    {
        get; protected set;
    }
    public bool Solved
    {
        get
        {
            return Shape != Tile.TileShape.DEFAULT;
        }
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
    public class ClickedEventArgs : EventArgs
    {
        public ClickedEventArgs(Tile tile)
        {
            _tile = tile;
        }
        public Tile _tile;
    }
    private Tile[,] _tiles;
    private Tile? _victoryTile;
}