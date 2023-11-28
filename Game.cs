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
        public void PlaceTile(Address address, Address subAddress)
        {
            Team team;
            if (_previousPlayer == 2)
            {
                _previousPlayer = 1;
                team = Teams[1];
                _ui.Activate(Teams[0]);
            }
            else
            {
                _previousPlayer = 2;
                team = Teams[0];
                _ui.Activate(Teams[1]);
            }
            Tile tile = _board.PlaceTile(team, address, subAddress);
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

            Address gridAddress = grid.FindAddress(tile);
            Address superGridAddress = superGrid.FindAddress(grid);
            if (superGrid.IsValidPlacement(superGridAddress, gridAddress))
            {
                PlaceTile(superGridAddress, gridAddress);
                if (_board.Team != null)
                {
                    _ui.AddPoints(_board.Team!, 1);
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
        }
        private int _previousPlayer;
        private readonly Team[] Teams;
        private UI _ui;
        private SuperGrid _board;
    }
}