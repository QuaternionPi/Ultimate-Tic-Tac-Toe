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
            _board = new Grid<Grid<Tile>>(null, transform, true, false);
            _board.Clicked += HandleClickedBoard;
            Teams = new Team[]{
                new Team(Tile.TileShape.X, Color.RED),
                new Team(Tile.TileShape.O, Color.BLUE)
            };
            _activeTeam = Teams[0];
            _ui = new UI(Teams);
        }
        protected void NextTeam()
        {
            if (_activeTeam == Teams[0])
            {
                _activeTeam = Teams[1];
            }
            else if (_activeTeam == Teams[1])
            {
                _activeTeam = Teams[0];
            }
        }
        public void HandleClickedBoard(ICell cell, IEnumerable<Address> from, bool placeable)
        {
            if (placeable == false)
            {
                return;
            }
            if (!cell.Equals(_board))
            {
                throw new Exception("Board click not from board");
            }
            Board = (Grid<Grid<Tile>>)Board.Place(from, _activeTeam, placeable, true);
            NextTeam();
            _ui.Activate(_activeTeam);
        }
        public void Draw()
        {
            Board.Draw();
            _ui.Draw();
        }
        public void Update()
        {
            Board.Update();
            if (_board.Team != null)
            {
                _ui.AddPoints(_board.Team, 1);
                Board = new Grid<Grid<Tile>>(null, Board.Transform, true, false);
            }
        }
        public Grid<Grid<Tile>> Board
        {
            get
            {
                return _board;
            }
            private set
            {
                _board.Clicked -= HandleClickedBoard;
                _board = value;
                _board.Clicked += HandleClickedBoard;
            }
        }
        private Grid<Grid<Tile>> _board;
        private Team _activeTeam;
        private readonly Team[] Teams;
        private UI _ui;
    }
}