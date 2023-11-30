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
        public Team? Team { get; }
        public bool Placeable { get; }
        public LinearTransform Transform { get; }
        public event ICell.ClickHandler? Clicked;
        public bool DrawGray { get; }
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
            Team.DrawSymbol(Transform, drawColor);
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
        public ICell Create(Team? team, LinearTransform transform, bool placeable, bool drawGray)
        {
            return new Tile(team, transform, placeable, drawGray);
        }
        public ICell Place(IEnumerable<Address> path, Team team, bool placeable, bool isRoot)
        {
            return new Tile(team, Transform, placeable, false);
        }
        public ICell DeepCopyPlacable(bool placeable)
        {
            return new Tile(Team, Transform, placeable, DrawGray);
        }
    }
}