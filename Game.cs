using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    class Game : IDrawable, IUpdateable
    {
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
        public void Draw()
        {
            if (_slideDownSpeed == 0)
            {
                _board.DrawPossibilities();
            }
            _board.Draw();
            _ui.Draw();
        }
        public void Update()
        {
            if (_board.Solved)
            {
                _board.SlideDown(_slideDownSpeed);
                _slideDownSpeed += 0.3f;
            }
            else
            {
                _board.Update();
            }
            if (_slideDownSpeed > 25)
            {
                _slideDownSpeed = 0;
                _board.Clicked -= HandleClickedBoard;
                _board = new SuperGrid(new Vector2(450, 350));
                _board.Clicked += HandleClickedBoard;
            }
            _ui.Update();
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
}