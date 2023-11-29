using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public class Tile : ICell
    {
        public enum TileShape { DEFAULT = 0, X, O };
        public Tile()
        {
            Team = null;
            Transform = new LinearTransform(Vector2.Zero, 0, 0);
            Placeable = false;
            DrawGray = true;
        }
        public Tile(Team? team, LinearTransform transform, bool placeable, bool drawGray)
        {
            Team = team;
            Transform = transform;
            Placeable = placeable && Team == null;
            DrawGray = drawGray;
        }
        public ICell Create(Team? team, LinearTransform transform, bool placeable, bool drawGray)
        {
            return new Tile(team, transform, placeable, drawGray);
        }
        public ICell Place(IEnumerable<Address> path, Team team, bool placeable, bool isRoot)
        {
            return new Tile(team, Transform, placeable, false);
        }
        public ICell Clone(bool placeable)
        {
            return new Tile(Team, Transform, placeable, DrawGray);
        }
        public void Update()
        {
            bool leftMouse = IsMouseButtonReleased(0);
            Vector2 mousePosition = GetMousePosition();
            Rectangle rectangle = new Rectangle(Transform.Position.X - 25, Transform.Position.Y - 25, 50, 50);
            bool collision = CheckCollisionPointRec(mousePosition, rectangle);
            if (leftMouse && collision)
            {
                Clicked?.Invoke(this, new Address[0], Placeable);
            }
        }
        public void Draw()
        {
            if (Placeable)
            {
                int width = 20;
                DrawRectangle((int)Transform.Position.X - width / 2, (int)Transform.Position.Y - width / 2, width, width, Color.LIGHTGRAY);
            }
            if (Team == null)
            {
                return;
            }
            Color drawColor;
            if (DrawGray)
            {
                drawColor = Color.GRAY;
            }
            else
            {
                drawColor = Team.Color;
            }
            switch (Team.Shape)
            {
                case TileShape.DEFAULT:
                    return;
                case TileShape.X:
                    {
                        int width = 5 * (int)Transform.Scale;
                        int length = 40 * (int)Transform.Scale;
                        Rectangle rectangle = new Rectangle(Transform.Position.X, Transform.Position.Y, width, length);
                        DrawRectanglePro(rectangle, new Vector2(width / 2, length / 2), 45, drawColor);
                        DrawRectanglePro(rectangle, new Vector2(width / 2, length / 2), -45, drawColor);
                        return;
                    }
                case TileShape.O:
                    {
                        int width = 4 * (int)Transform.Scale;
                        int innerRadius = 13 * (int)Transform.Scale;
                        int outerRadius = innerRadius + width;

                        DrawRing(Transform.Position, innerRadius, outerRadius, 0, 360, 50, drawColor);
                        return;
                    }
            }
        }
        public Team? Team { get; }
        public LinearTransform Transform { get; }
        public event ICell.ClickHandler? Clicked;
        public bool Placeable { get; }
        public bool DrawGray { get; }
    }
}