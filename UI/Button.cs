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
        public class Button : IDrawable, IUpdateable
        {
            public Button(
                LinearTransform transform,
                Vector2 dimensions,
                string message,
                Color textColor,
                Color backgroundColor,
                Color borderColor)
            {
                Transform = transform;
                Dimensions = dimensions;
                Message = message;
                TextColor = textColor;
                BackgroundColor = backgroundColor;
                BorderColor = borderColor;
            }
            public LinearTransform Transform { get; }
            protected Vector2 Dimensions { get; }
            public string Message { get; set; }
            public Color TextColor { get; set; }
            public Color BackgroundColor { get; set; }
            public Color BorderColor { get; set; }
            public delegate void Click();
            public event Click? Clicked;
            public void Update()
            {
                bool leftMouse = IsMouseButtonReleased(0);
                Vector2 mousePosition = GetMousePosition();
                Rectangle rectangle = new Rectangle(
                    Transform.Position.X - Dimensions.X / 2,
                    Transform.Position.Y - Dimensions.Y / 2,
                    Dimensions.X,
                    Dimensions.Y
                );
                bool collision = CheckCollisionPointRec(mousePosition, rectangle);
                if (leftMouse && collision)
                {
                    Clicked?.Invoke();
                }
            }
            public void Draw()
            {
                DrawBox();
                DrawMessage();
            }
            protected void DrawBox()
            {
                int width = (int)Dimensions.X;
                int height = (int)Dimensions.Y;
                int x = (int)Transform.Position.X - width / 2;
                int y = (int)Transform.Position.Y - height / 2;
                Rectangle rectangle = new Rectangle(x, y, width, height);
                DrawRectangleRec(rectangle, BackgroundColor);
                DrawRectangleLinesEx(rectangle, 5, BorderColor);
            }
            protected void DrawMessage()
            {
                float spacing = 3;
                float fontSize = 40;
                Vector2 messageDimensions = MeasureTextEx(GetFontDefault(), Message, fontSize, spacing);
                Vector2 drawPosition = Transform.Position - messageDimensions / 2;
                DrawTextEx(GetFontDefault(), Message, drawPosition, fontSize, spacing, TextColor);
            }
        }
    }
}