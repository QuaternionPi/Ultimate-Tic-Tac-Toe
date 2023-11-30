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
        public enum Symbol { X, O };
        public Team(Symbol symbol, Color color)
        {
            Shape = symbol;
            Color = color;
        }
        public Symbol Shape { get; protected set; }
        public Color Color { get; protected set; }
        public void Draw(LinearTransform transform, Color color)
        {

            switch (Shape)
            {
                case Symbol.X:
                    {
                        int width = 5 * (int)transform.Scale;
                        int length = 40 * (int)transform.Scale;
                        Rectangle rectangle = new Rectangle(transform.Position.X, transform.Position.Y, width, length);
                        DrawRectanglePro(rectangle, new Vector2(width / 2, length / 2), 45, color);
                        DrawRectanglePro(rectangle, new Vector2(width / 2, length / 2), -45, color);
                        return;
                    }
                case Symbol.O:
                    {
                        int width = 4 * (int)transform.Scale;
                        int innerRadius = 13 * (int)transform.Scale;
                        int outerRadius = innerRadius + width;

                        DrawRing(transform.Position, innerRadius, outerRadius, 0, 360, 50, color);
                        return;
                    }
            }
        }
    }
}