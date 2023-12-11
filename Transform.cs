using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe
{
    public readonly struct Transform2D
    {
        public Transform2D(Vector2 position, float rotation = 0, float scale = 1)
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
        public Transform2D Translate(Vector2 delta)
        {
            return new Transform2D(Position + delta, Rotation, Scale);
        }
    }
}