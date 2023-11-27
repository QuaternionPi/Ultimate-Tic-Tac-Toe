using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    class Team
    {
        public Team(Tile.TileShape shape, Color color)
        {
            Shape = shape;
            Color = color;
        }
        protected Tile.TileShape Shape;
        protected Color Color;
    }
}