using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public readonly struct Address
    {
        public Address(int x, int y)
        {
            if (x > 2 | x < 0 | y > 2 | y < 0)
            {
                throw new IndexOutOfRangeException("Position not in grid");
            }
            X = x;
            Y = y;
        }
        public int X { get; }
        public int Y { get; }
    }
}