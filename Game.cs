using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public class Game : IDrawable, IUpdateable
    {
        public Game()
        {
            Vector2 position = new Vector2(450, 350);
            LinearTransform transform = new LinearTransform(position, 0, 4);
            _board = new SuperGrid(transform);
            _board.Clicked += HandleClickedBoard;
            _previousPlayer = 0;
            Teams = new Team[]{
                new Team(Tile.TileShape.X, Color.RED),
                new Team(Tile.TileShape.O, Color.BLUE)
            };
            _ui = new UI(Teams);
        }
        public void PlaceTile(Vector2 position, Vector2 subPosition)
        {
            Team team;
            if (_previousPlayer == 2)
            {
                _previousPlayer = 1;
                team = Teams[1];
            }
            else
            {
                _previousPlayer = 2;
                team = Teams[0];
            }
            Tile tile = _board.PlaceTile(team, position, subPosition);
            _ui.Activate(team.Shape);
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
                if (_board.Team != null)
                {
                    _ui.IncrimentScore(_board.Team.Shape);
                }
            }
        }
        public void Draw()
        {
            _board.DrawPossibilities();
            _board.Draw();
            _ui.Draw();
        }
        public void Update()
        {
            _board.Update();
            if (_board.Team != null)
            {
                _board.Clicked -= HandleClickedBoard;
                _board = new SuperGrid(_board.Transform);
                _board.Clicked += HandleClickedBoard;
            }
            _ui.Update();
        }
        private int _previousPlayer;
        private readonly Team[] Teams;
        private UI _ui;
        private SuperGrid _board;
    }
}