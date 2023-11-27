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
            _drawGray = false;
        }
        public Tile(Team? team, LinearTransform transform)
        {
            Team = team;
            Transform = transform;
            _drawGray = false;
        }
        public void Update()
        {
            bool leftMouse = IsMouseButtonReleased(0);
            Vector2 mousePosition = GetMousePosition();
            Rectangle rectangle = new Rectangle(Transform.Position.X - 25, Transform.Position.Y - 25, 50, 50);
            bool collision = CheckCollisionPointRec(mousePosition, rectangle);
            if (leftMouse && collision)
            {
                Clicked?.Invoke(this, EventArgs.Empty);
            }
        }
        public void Draw()
        {
            if (Team == null)
            {
                return;
            }
            Color drawColor;
            if (_drawGray)
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
        public Team? Winner()
        {
            return Team;
        }
        public Team? Team
        {
            get
            {
                return _team;
            }
            set
            {
                if (_team != null) { throw new Exception("Cannot reassign Tile team"); }
                _team = value;
            }
        }
        public LinearTransform Transform { get; protected set; }
        public event EventHandler? Clicked;
        protected Team? _team;
        public bool _drawGray;
    }
}