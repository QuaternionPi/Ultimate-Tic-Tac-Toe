using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe
{
    namespace Game
    {
        public class Game : IDrawable, IUpdateable
        {
            public Game(Player player1, Player player2, Vector2 position)
            {
                Players = new Player[] { player1, player2 };

                LinearTransform transform = new LinearTransform(position, 0, 4);
                _board = new Grid<Grid<Tile>>(null, transform, true);

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
            protected bool ChangePlayer;
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
                TimeSpan delay = new(0, 0, 0, 0, 100);
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
                ChangePlayer = true;
            }
            protected void DelayedPlayerStart(TimeSpan delay)
            {
                Thread.Sleep(delay);
                ActivePlayer.BeginTurn(Board, InactivePlayer);
            }
            public void Draw()
            {
                Board.Draw();
                BannerControler.Draw();
            }
            public void Update()
            {
                Board.Update();
                if (ChangePlayer && Board.Placeable)
                {
                    NextPlayer();
                    ChangePlayer = false;
                }
                ActivePlayer.Update();
                // Board currently in a transition
                if (Board.TransitionValue != 0)
                {
                    return;
                }
                // Board won by a player
                if (Board.Player != null)
                {
                    Board.Player.Score += 1;
                    Board = new Grid<Grid<Tile>>(null, Board.Transform, true);
                }
                // Board is tied
                if (Board.Player == null && Board.Placeable == false)
                {
                    Board = new Grid<Grid<Tile>>(null, Board.Transform, true);
                }
                // Toggle players
                BannerControler.Activate(ActivePlayer);
                BannerControler.Deactivate(InactivePlayer);
            }
        }
    }
}