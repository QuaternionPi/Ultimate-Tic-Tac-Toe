using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public abstract class Player : IUpdateable
    {
        public enum Symbol { X, O };
        public Player(Symbol symbol, Color color)
        {
            Shape = symbol;
            Color = color;
        }
        public Symbol Shape { get; protected set; }
        public Color Color { get; protected set; }
        public delegate void Turn(Player player, IEnumerable<ICell> cells);
        public event Turn? PlayTurn;
        protected void InvokePlayTurn(Player player, IEnumerable<ICell> cells) => PlayTurn?.Invoke(player, cells);
        public abstract void BeginTurn(Grid<Grid<Tile>> board, Player opponent);
        public abstract void EndTurn();
        public abstract void Update();
        public void DrawSymbol(LinearTransform transform, float transitionValue, Color color)
        {
            transitionValue = Math.Clamp(transitionValue, 0, 1);
            switch (Shape)
            {
                case Symbol.X:
                    {
                        Vector2 maxDimensions = new Vector2(40, 5) * transform.Scale;
                        float leftLength = maxDimensions.X * Math.Clamp(1 - transitionValue, 0, 0.5f) * 2;
                        float rightLength = maxDimensions.X * (Math.Clamp(1 - transitionValue, 0.5f, 1) - 0.5f) * 2;
                        Rectangle leftRectangle = new Rectangle(
                            transform.Position.X,
                            transform.Position.Y,
                            leftLength,
                            maxDimensions.Y);
                        Rectangle rightRectangle = new Rectangle(
                            transform.Position.X,
                            transform.Position.Y,
                            rightLength,
                            maxDimensions.Y);

                        DrawRectanglePro(leftRectangle, maxDimensions / 2, 45, color);
                        DrawRectanglePro(rightRectangle, maxDimensions / 2, -45, color);
                        return;
                    }
                case Symbol.O:
                    {
                        int width = 4 * (int)transform.Scale;
                        int innerRadius = 11 * (int)transform.Scale;
                        int outerRadius = innerRadius + width;
                        float angle = 360f * (1 - transitionValue) + 180;

                        DrawRing(transform.Position, innerRadius, outerRadius, 180, angle, 50, color);
                        return;
                    }
            }
        }
    }
}