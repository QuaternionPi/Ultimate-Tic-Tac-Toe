using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public class Team
    {
        public Team(Tile.TileShape shape, Color color)
        {
            Shape = shape;
            Color = color;
        }
        public Tile.TileShape Shape { get; protected set; }
        public Color Color { get; protected set; }
    }
}