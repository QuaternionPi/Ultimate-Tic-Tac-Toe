using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

class Tile : IDrawable, IUpdateable, ITransform
{
    public enum TileShape { DEFAULT = 0, X, O };
    private Tile(TileShape shape, Color color)
    {
        Position = Vector2.Zero;
        Rotation = 0;
        Scale = 1;
        Shape = shape;
        _color = color;
        _drawGray = false;
    }
    public static Tile defaultTile() => new Tile(TileShape.DEFAULT, Color.WHITE);
    public static Tile xTile() => new Tile(TileShape.X, Color.RED);
    public static Tile oTile() => new Tile(TileShape.O, Color.BLUE);
    public void SlideDown(float deltaY)
    {
        Position = new Vector2(Position.X, Position.Y - deltaY);
    }
    public void Update()
    {
        bool leftMouse = IsMouseButtonReleased(0);
        Vector2 mousePosition = GetMousePosition();
        Rectangle rectangle = new Rectangle(Position.X - 25, Position.Y - 25, 50, 50);
        bool collision = CheckCollisionPointRec(mousePosition, rectangle);
        if (leftMouse && collision)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
    public void Draw()
    {
        Color drawColor;
        if (_drawGray)
        {
            drawColor = Color.GRAY;
        }
        else
        {
            drawColor = _color;
        }
        switch (Shape)
        {
            case TileShape.DEFAULT:
                return;
            case TileShape.X:
                {
                    int width = 5 * (int)Scale;
                    int length = 40 * (int)Scale;
                    Rectangle rectangle = new Rectangle(Position.X, Position.Y, width, length);
                    DrawRectanglePro(rectangle, new Vector2(width / 2, length / 2), 45, drawColor);
                    DrawRectanglePro(rectangle, new Vector2(width / 2, length / 2), -45, drawColor);
                    return;
                }
            case TileShape.O:
                {
                    int width = 4 * (int)Scale;
                    int innerRadius = 13 * (int)Scale;
                    int outerRadius = innerRadius + width;

                    DrawRing(Position, innerRadius, outerRadius, 0, 360, 50, drawColor);
                    return;
                }
        }
    }
    public Vector2 Position
    {
        get; set;
    }
    public float Rotation
    {
        get; protected set;
    }
    public float Scale
    {
        get; set;
    }
    public TileShape Shape
    {
        get; protected set;
    }
    public event EventHandler? Clicked;
    protected Color _color;
    public bool _drawGray;
}