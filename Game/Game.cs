using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe
{
    namespace Game
    {
        public class Game : IDrawable, IUpdateable
        {
            public Game(Player active, Player inactve, Vector2 position)
            {
                Transform2D transform = new Transform2D(position, 0, 4);
                _board = new Grid<Grid<Tile>>(null, transform, true);

                ActivePlayer = active;
                InactivePlayer = inactve;
                ActivePlayer.PlayTurn += HandlePlayerTurn;
                InactivePlayer.PlayTurn += HandlePlayerTurn;
                BannerControler = new UI.BannerControler(active, inactve);


                TimeSpan delay = new(0, 0, 0, 0, 100);
                Thread thread = new(() => DelayedPlayerStart(delay));
                thread.Start();
            }
            [JsonInclude]
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
            [JsonInclude]
            public Player ActivePlayer { get; protected set; }
            [JsonInclude]
            public Player InactivePlayer { get; protected set; }
            protected bool ChangePlayer;
            [JsonInclude]
            public UI.BannerControler BannerControler { get; protected set; }
            protected void NextPlayer()
            {
                var mini = new Grid<Tile>();

                string json = JsonSerializer.Serialize(mini);
                Player temp;
                ActivePlayer.EndTurn();

                temp = ActivePlayer;
                ActivePlayer = InactivePlayer;
                InactivePlayer = temp;

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