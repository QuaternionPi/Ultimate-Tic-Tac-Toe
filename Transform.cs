using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;


namespace UltimateTicTacToe
{
    public interface ITransform
    {
        Vector2 Position
        {
            get;
        }
        float Rotation
        {
            get;
        }
        float Scale
        {
            get;
        }
    }
    public readonly struct LinearTransform : ITransform
    {
        public LinearTransform(Vector2 position, float rotation = 0, float scale = 1)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
        public Vector2 Position
        {
            get;
        }
        public float Rotation
        {
            get;
        }
        public float Scale
        {
            get;
        }
        public LinearTransform Translate(Vector2 delta)
        {
            return new LinearTransform(Position + delta, Rotation, Scale);
        }
    }
}