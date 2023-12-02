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
                new Team(Team.Symbol.X, Color.RED),
                new Team(Team.Symbol.O, Color.BLUE)
            };
            ActiveTeam = Teams[0];
            BannerControler = new BannerControler(Teams);
        }
        public Grid<Grid<Tile>> Board
        {
            get
            {
                return _board;
            }
            protected set
            {
                _board.Clicked -= HandleClickedBoard;
                _board = value;
                _board.Clicked += HandleClickedBoard;
            }
        }
        private Grid<Grid<Tile>> _board;
        protected Team ActiveTeam;
        protected readonly Team[] Teams;
        protected BannerControler BannerControler;
        protected void NextTeam()
        {
            if (ActiveTeam == Teams[0])
            {
                ActiveTeam = Teams[1];
            }
            else if (ActiveTeam == Teams[1])
            {
                ActiveTeam = Teams[0];
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
            Board = (Grid<Grid<Tile>>)Board.Place(from, ActiveTeam, placeable, true);
            NextTeam();
            BannerControler.Activate(ActiveTeam);
        }
        public void Draw()
        {
            Board.Draw();
            BannerControler.Draw();
        }
        public void Update()
        {
            Board.Update();
            if (_board.Team != null)
            {
                BannerControler.AddPoints(_board.Team, 1);
                Board = new Grid<Grid<Tile>>(null, Board.Transform, true, false);
            }
        }
    }
}