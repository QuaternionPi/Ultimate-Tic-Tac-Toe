using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;
using UltimateTicTacToe.UI;

namespace UltimateTicTacToe
{
    static class Program
    {
        static void Main(string[] args)
        {
            SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
            InitWindow(900, 650, "Ultimate Tic Tac Toe");
            SetTargetFPS(30);

            Mode = new Setup();
            Mode.SwitchTo += ChangeMode;

            while (!WindowShouldClose())
            {
                Mode.Update();
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                Mode.Draw();
                EndDrawing();
            }
        }
        private static IProgramMode Mode;
        private static void ChangeMode(IProgramMode from, IProgramMode to)
        {
            from.SwitchTo -= ChangeMode;
            Mode = to;
            to.SwitchTo += ChangeMode;
        }
    }
    public interface IProgramMode : IDrawable, IUpdateable, ITransitionable
    {
        public event SwitchToDel? SwitchTo;
        public delegate void SwitchToDel(IProgramMode sender, IProgramMode switchTo);
        public IProgramMode? Previous { get; }
    }
    public class Setup : IProgramMode
    {
        public Setup()
        {
            Player1 = new Human(Player.Symbol.X, Color.RED);
            Player2 = new Bot(Player.Symbol.O, Color.BLUE);
            UI = new UI.BannerControler(new Player[] { Player1, Player2 });
            UI.Activate(Player1);
            UI.Activate(Player2);

            LinearTransform transform = new(new Vector2(450, 300));
            Button = new Button(transform, new Vector2(400, 100), "Play", Color.LIGHTGRAY);
            Button.Clicked += SetupGame;
        }
        protected UI.Button Button;
        public bool InTransition { get; }
        public float TransitionValue { get; }
        public event IProgramMode.SwitchToDel? SwitchTo;
        public IProgramMode? Previous { get; }
        protected UI.BannerControler UI;
        protected Player Player1;
        protected Player Player2;
        public void Draw()
        {
            Button.Draw();
            UI.Draw();
        }
        public void Update()
        {
            Button.Update();
        }
        protected void SetupGame()
        {
            Player player1 = new Human(Player.Symbol.X, Color.RED);
            Player player2 = new Bot(Player.Symbol.O, Color.BLUE);
            IProgramMode mode = new PlayGame(player1, player2);
            SwitchTo?.Invoke(this, mode);
        }
    }
    public class Save
    {

    }
    public class Load
    {

    }
    public class PlayGame : IProgramMode
    {
        public PlayGame(Player player1, Player player2)
        {

            Vector2 position = new Vector2(450, 350);
            Game = new Game.Game(player1, player2, position);
        }
        public bool InTransition { get; }
        public float TransitionValue { get; }
        public event IProgramMode.SwitchToDel? SwitchTo;
        public IProgramMode? Previous { get; }
        protected Game.Game Game;
        public void Draw()
        {
            Game.Draw();
        }
        public void Update()
        {
            Game.Update();
        }
    }
}