using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;


namespace UltimateTicTacToe
{
    class Button : IDrawable, IUpdateable, ITransform
    {
        public Button(Vector2 position, float width, float height, string message, Color color)
        {
            Position = position;
            _width = width;
            _height = height;
            _message = message;
            _color = color;
        }
        public void Update()
        {

        }
        public void Draw()
        {
            int width = (int)_width;
            int height = (int)_height;
            int x = (int)Position.X - width / 2;
            int y = (int)Position.Y - height / 2;
            DrawRectangle(x, y, width, height, _color);
        }
        public Vector2 Position
        {
            get; set;
        }
        public float Rotation
        {
            get; protected set;
        }
        public float Scale
        {
            get; set;
        }
        public event EventHandler? Clicked;
        private string _message;
        private Color _color;
        private float _width;
        private float _height;
    }
}