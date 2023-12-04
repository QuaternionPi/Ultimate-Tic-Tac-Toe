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
            Players = new Player[]{
                new Human(Player.Symbol.X, Color.RED),
                new Human(Player.Symbol.O, Color.BLUE)
            };
            ActivePlayer = Players[0];
            ActivePlayer.BeginTurn(Board);
            BannerControler = new UI.BannerControler(Players);
            foreach (Player player in Players)
            {
                player.PlayTurn += HandlePlayerTurn;
            }
        }
        public Grid<Grid<Tile>> Board
        {
            get
            {
                return _board;
            }
            protected set
            {
                ActivePlayer.EndTurn();
                _board = value;
                ActivePlayer.BeginTurn(Board);
            }
        }
        private Grid<Grid<Tile>> _board;
        protected Player ActivePlayer;
        protected readonly Player[] Players;
        protected UI.BannerControler BannerControler;
        protected void NextPlayer()
        {
            ActivePlayer.EndTurn();
            if (ActivePlayer == Players[0])
            {
                ActivePlayer = Players[1];
            }
            else if (ActivePlayer == Players[1])
            {
                ActivePlayer = Players[0];
            }
            ActivePlayer.BeginTurn(Board);
        }
        public void HandlePlayerTurn(Player player, IEnumerable<ICell> cells)
        {
            if (player != ActivePlayer)
            {
                Console.WriteLine($"Not player {player}'s turn");
                return;
            }
            Board = (Grid<Grid<Tile>>)Board.Place(cells.Skip(1), ActivePlayer, true);
            NextPlayer();
            BannerControler.Activate(ActivePlayer);
        }
        public void Draw()
        {
            Board.Draw();
            BannerControler.Draw();
        }
        public void Update()
        {
            Board.Update();
            if (_board.Player != null)
            {
                BannerControler.AddPoints(_board.Player, 1);
                Board = new Grid<Grid<Tile>>(null, Board.Transform, true, false);
            }
            ActivePlayer.Update();
        }
    }
}