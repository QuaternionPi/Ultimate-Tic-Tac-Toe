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
            _board = new Grid<Grid<Tile>>(null, transform, true);
            Players = new Player[]{
                new Human(Player.Symbol.X, Color.RED),
                new Bot(Player.Symbol.O, Color.BLUE)
            };

            ActivePlayer = Players[0];
            InactivePlayer = Players[1];
            BannerControler = new UI.BannerControler(Players);

            foreach (Player player in Players)
            {
                player.PlayTurn += HandlePlayerTurn;
            }

            TimeSpan delay = new(0, 0, 1);
            Thread thread = new(() => DelayedPlayerStart(delay));
            thread.Start();
        }
        public Grid<Grid<Tile>> Board
        {
            get
            {
                return _board;
            }
            protected set
            {
                _board = value;
            }
        }
        private Grid<Grid<Tile>> _board;
        protected Player ActivePlayer;
        protected Player InactivePlayer;
        protected readonly Player[] Players;
        protected UI.BannerControler BannerControler;
        protected void NextPlayer()
        {
            ActivePlayer.EndTurn();
            if (ActivePlayer == Players[0])
            {
                ActivePlayer = Players[1];
                InactivePlayer = Players[0];
            }
            else if (ActivePlayer == Players[1])
            {
                ActivePlayer = Players[0];
                InactivePlayer = Players[1];
            }
            TimeSpan delay = new(0, 0, 1);
            Thread thread = new(() => DelayedPlayerStart(delay));
            thread.Start();
        }
        protected void HandlePlayerTurn(Player player, IEnumerable<ICell> cells)
        {
            if (player != ActivePlayer)
            {
                Console.WriteLine($"Not player {player}'s turn");
                return;
            }
            Board = (Grid<Grid<Tile>>)Board.Place(cells.Skip(1), ActivePlayer, true);
            if (Board.Player == null && Board.Placeable == false)
            {
                Board = new Grid<Grid<Tile>>(null, Board.Transform, true);
            }
            if (Board.Player == null)
                NextPlayer();
            BannerControler.Activate(ActivePlayer);
        }
        protected void DelayedPlayerStart(TimeSpan delay)
        {
            System.Diagnostics.Stopwatch stopwatch = new();
            stopwatch.Start();
            ActivePlayer.BeginTurn(Board, InactivePlayer);
            stopwatch.Stop();
            TimeSpan timeRemaining = delay - stopwatch.Elapsed;
            if (timeRemaining.CompareTo(new TimeSpan(0, 0, 0)) > 0)
                Thread.Sleep(timeRemaining);
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
                Board = new Grid<Grid<Tile>>(null, Board.Transform, true);
                NextPlayer();
            }
            ActivePlayer.Update();
        }
    }
}