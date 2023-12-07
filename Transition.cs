using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public interface ITransitionable
    {
        public bool InTransition { get; }
        public float TransitionValue { get; }
    }
}