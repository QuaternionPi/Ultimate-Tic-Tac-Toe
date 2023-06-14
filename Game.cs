using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

class Game : GameObject
{
    protected class UI : GameObject
    {
        protected class Banner : GameObject, ITransform
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
            public override void Update()
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
            public override void Draw()
            {
                string message = _score.ToString();
                float spacing = 3;
                float fontSize = 80;
                float messageWidth = MeasureTextEx(_font, message, fontSize, spacing).X;
                Vector2 drawPosition = Position + new Vector2(-messageWidth / 2 + 75, 170);
                DrawTextEx(_font, message, drawPosition, fontSize, spacing, Color.LIGHTGRAY);
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
        public override void Draw()
        {
            string message = "Ultimate Tic Tac Toe";
            float spacing = 3;
            float fontSize = 30;
            float messageWidth = MeasureTextEx(_font, message, fontSize, spacing).X;
            DrawTextEx(_font, message, new Vector2(450 - messageWidth / 2, 20), fontSize, spacing, Color.GRAY);
        }
        Banner _leftBanner;
        Banner _rightBanner;
        Font _font;
    }
    public Game()
    {
        Vector2 position = new Vector2(450, 350);
        _board = new SuperGrid(position);
        _board.Clicked += HandleClickedBoard;
        _previousPlayer = 0;
        _ui = new UI();
        _slideDownSpeed = 0;
        PreviousTile = Tile.defaultTile();
    }
    public void PlaceTile(Vector2 position, Vector2 subPosition)
    {
        Tile tile;
        if (_previousPlayer == 2)
        {
            _previousPlayer = 1;
            tile = Tile.oTile();
        }
        else
        {
            _previousPlayer = 2;
            tile = Tile.xTile();
        }
        _board.PlaceTile(tile, position, subPosition);
        _ui.Activate(PreviousTile.Shape);
        PreviousTile = tile;
    }
    public void HandleClickedBoard(object? sender, SuperGrid.ClickedEventArgs args)
    {
        if (sender == null)
        {
            return;
        }
        Tile tile = args._tile;
        Grid grid = args._grid;
        SuperGrid superGrid = (SuperGrid)sender;

        Vector2 gridPosition = grid.GridPosition(tile);
        Vector2 superGridPosition = superGrid.GridPosition(grid);
        if (superGrid.IsValidPlacement(superGridPosition, gridPosition))
        {
            PlaceTile(superGridPosition, gridPosition);
            if (_board.Solved)
            {
                _ui.IncrimentScore(_board.Shape);
            }
        }
    }
    public override void Update()
    {
        if (_board.Solved)
        {
            _board.SlideDown(_slideDownSpeed);
            _slideDownSpeed += 0.3f;
        }
        if (_slideDownSpeed > 25)
        {
            _slideDownSpeed = 0;
            _board.Clicked -= HandleClickedBoard;
            _board.Destroy();
            _board = new SuperGrid(new Vector2(450, 350));
            _board.Clicked += HandleClickedBoard;
        }
    }
    public override void Draw()
    {
        if (_slideDownSpeed == 0)
        {
            _board.DrawPossibilities();
        }
    }
    private float _slideDownSpeed;
    private int _previousPlayer;
    public Tile PreviousTile
    {
        get; private set;
    }
    private UI _ui;
    private SuperGrid _board;
}