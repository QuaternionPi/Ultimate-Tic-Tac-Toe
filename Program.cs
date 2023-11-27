﻿using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            InitWindow(900, 650, "Ultimate Tic Tac Toe");
            SetTargetFPS(30);
            Game game = new Game();
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
}