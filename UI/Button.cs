using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;


namespace UltimateTicTacToe
{
    namespace UI
    {
        class Button : IDrawable, IUpdateable
        {
            public Button(LinearTransform transform, Vector2 dimensions, string message, Color color)
            {
                Transform = transform;
                Dimensions = dimensions;
                Message = message;
                Color = color;
            }
            public LinearTransform Transform { get; }
            public event EventHandler? Clicked;
            private string Message { get; }
            protected Color Color { get; }
            protected Vector2 Dimensions { get; }
            public void Update()
            {

            }
            public void Draw()
            {
                int width = (int)Dimensions.X;
                int height = (int)Dimensions.Y;
                int x = (int)Transform.Position.X - width / 2;
                int y = (int)Transform.Position.Y - height / 2;
                DrawRectangle(x, y, width, height, Color);
            }
        }
    }
}