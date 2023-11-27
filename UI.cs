using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;
class UI : IDrawable
{
    protected class Banner : IDrawable, IUpdateable, ITransform
    {
        public Banner(Tile tile, Vector2 position)
        {
            _font = GetFontDefault();
            _active = tile.Shape == Tile.TileShape.X;
            _tile = tile;
            Position = position;
            _tile.Position = Position + new Vector2(75, 75);
            _tile.Scale *= 3;
            _score = 0;
        }
        public void Update()
        {
            if (_active)
            {
                _tile._drawGray = false;
            }
            else
            {
                _tile._drawGray = true;
            }
        }
        public void Draw()
        {
            string message = _score.ToString();
            float spacing = 3;
            float fontSize = 80;
            float messageWidth = MeasureTextEx(_font, message, fontSize, spacing).X;
            Vector2 drawPosition = Position + new Vector2(-messageWidth / 2 + 75, 170);
            DrawTextEx(_font, message, drawPosition, fontSize, spacing, Color.LIGHTGRAY);
            _tile.Draw();
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
        public bool _active;
        protected Tile _tile;
        public int _score;
        Font _font;
    }
    public UI()
    {
        _font = GetFontDefault();
        _leftBanner = new Banner(Tile.xTile(), new Vector2(0, 0));
        _rightBanner = new Banner(Tile.oTile(), new Vector2(750, 0));
    }
    public void Activate(Tile.TileShape shape)
    {
        if (shape == Tile.TileShape.X)
        {
            _leftBanner._active = true;
            _rightBanner._active = false;
        }
        else
        {
            _rightBanner._active = true;
            _leftBanner._active = false;
        }
    }
    public void IncrimentScore(Tile.TileShape shape)
    {
        if (shape == Tile.TileShape.X)
        {
            _leftBanner._score++;
        }
        else
        {
            _rightBanner._score++;
        }
    }
    public void Draw()
    {
        string message = "Ultimate Tic Tac Toe";
        float spacing = 3;
        float fontSize = 30;
        float messageWidth = MeasureTextEx(_font, message, fontSize, spacing).X;
        DrawTextEx(_font, message, new Vector2(450 - messageWidth / 2, 20), fontSize, spacing, Color.GRAY);
        _leftBanner.Draw();
        _rightBanner.Draw();
    }
    public void Update()
    {
        _leftBanner.Update();
        _rightBanner.Update();
    }
    Banner _leftBanner;
    Banner _rightBanner;
    Font _font;
}