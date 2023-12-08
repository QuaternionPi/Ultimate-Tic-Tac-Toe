using System.Numerics;
using Raylib_cs;

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
                bool leftMouse = Mouse.IsMouseButtonReleased(0);
                Vector2 mousePosition = Mouse.GetMousePosition();
                Rectangle rectangle = new Rectangle(
                    Transform.Position.X - Dimensions.X / 2,
                    Transform.Position.Y - Dimensions.Y / 2,
                    Dimensions.X,
                    Dimensions.Y
                );
                bool collision = CheckCollision.PointRec(mousePosition, rectangle);
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
                Graphics.Draw.RectangleRec(rectangle, BackgroundColor);
                Graphics.Draw.RectangleLinesEx(rectangle, 5, BorderColor);
            }
            protected void DrawMessage()
            {
                float spacing = 3;
                float fontSize = 40;
                Vector2 messageDimensions = Graphics.Text.MeasureTextEx(Graphics.Text.GetFontDefault(), Message, fontSize, spacing);
                Vector2 drawPosition = Transform.Position - messageDimensions / 2;
                Graphics.Text.DrawTextEx(Graphics.Text.GetFontDefault(), Message, drawPosition, fontSize, spacing, TextColor);
            }
        }
    }
}