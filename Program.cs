using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    static class Program
    {
        static void Main(string[] args)
        {
            SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
            InitWindow(900, 650, "Ultimate Tic Tac Toe");
            SetTargetFPS(30);
            Game.Game game = new Game.Game();
            while (!WindowShouldClose())
            {
                game.Update();
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                game.Draw();
                EndDrawing();
            }
        }
    }
    public interface IProgramMode : IDrawable, IUpdateable, ITransitionable
    {
        public event SwitchToDel? SwitchTo;
        public delegate void SwitchToDel(IProgramMode sender, IProgramMode switchTo);
    }
}