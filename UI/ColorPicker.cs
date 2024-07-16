using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe.UI;
/*
UI Element for picking colors
*/
public class ColorPicker : IDrawable, IUpdateable
{
    public ColorPicker(Transform2D transform, Color[] colors, int buttonsPerRow)
    {
        Transform = transform;
        List<ColorSelectionButton> colorButtons = new();
        int buttonSpacing = 50;
        int buttonsInRow = 0;
        var buttonTransform = new Transform2D(transform.Position, 0, 1);
        foreach (Color color in colors)
        {
            var button = new ColorSelectionButton(buttonTransform, color);
            button.Clicked += ColorButtonClicked;
            colorButtons.Add(button);
            buttonsInRow += 1;
            if (buttonsInRow == buttonsPerRow)
            {
                buttonsInRow = 0;
                Vector2 newButtonPosition = new Vector2(Transform.Position.X, buttonTransform.Position.Y + buttonSpacing);
                buttonTransform = new Transform2D(newButtonPosition, 0, Transform.Scale);
            }
            else
            {
                Vector2 newButtonPosition = new Vector2(buttonTransform.Position.X + buttonSpacing, buttonTransform.Position.Y);
                buttonTransform = new Transform2D(newButtonPosition, 0, Transform.Scale);
            }
        }
        Buttons = colorButtons.ToArray();
    }
    public Transform2D Transform { get; }
    private ColorSelectionButton[] Buttons { get; }
    private void ColorButtonClicked(Color color) => Clicked?.Invoke(color);
    public delegate void Click(Color color);
    public event Click? Clicked;
    public void Draw()
    {
        foreach (ColorSelectionButton button in Buttons)
        {
            button.Draw();
        }
    }

    public void Update()
    {
        foreach (ColorSelectionButton button in Buttons)
        {
            button.Update();
        }
    }
    private class ColorSelectionButton : IDrawable, IUpdateable
    {
        public ColorSelectionButton(Transform2D transform, Color color)
        {
            Transform = transform;
            Color = color;
            Radius = 14;
        }
        public Transform2D Transform { get; protected set; }
        public Color Color { get; protected set; }
        protected float Radius;
        public delegate void Click(Color color);
        public event Click? Clicked;
        public void Draw()
        {
            Graphics.Draw.CircleV(Transform.Position, Radius + 5, Color.LIGHTGRAY);
            Graphics.Draw.CircleV(Transform.Position, Radius + 2, Color.RAYWHITE);
            Graphics.Draw.CircleV(Transform.Position, Radius, Color);
            Graphics.Draw.CircleV(Transform.Position + new Vector2(0, -1), Radius - 1, Color);
            Graphics.Draw.CircleV(Transform.Position + new Vector2(0, 1), Radius - 1, Color);
        }
        public void Update()
        {
            bool leftMouse = Mouse.IsMouseButtonReleased(0);
            Vector2 mousePosition = Mouse.GetMousePosition();
            bool collision = CheckCollision.PointCircle(mousePosition, Transform.Position, Radius);
            if (leftMouse && collision)
            {
                Clicked?.Invoke(Color);
            }
        }
    }
}