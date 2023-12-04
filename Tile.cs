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
            Player = null;
            Transform = new LinearTransform(Vector2.Zero, 0, 0);
            Placeable = false;
            DrawGray = true;
        }
        public Tile(Player? player, LinearTransform transform, bool placeable, bool drawGray)
        {
            Player = player;
            Transform = transform;
            Placeable = placeable && Player == null;
            DrawGray = drawGray;
        }
        public Player? Player { get; }
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
            if (Player == null)
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
                drawColor = Player.Color;
            }
            Player.DrawSymbol(Transform, drawColor);
        }
        public void Update()
        {
            bool leftMouse = IsMouseButtonReleased(0);
            Vector2 mousePosition = GetMousePosition();
            Rectangle rectangle = new Rectangle(Transform.Position.X - 25, Transform.Position.Y - 25, 50, 50);
            bool collision = CheckCollisionPointRec(mousePosition, rectangle);
            if (leftMouse && collision)
            {
                var cells = new List<ICell>() { this };
                Clicked?.Invoke(cells);
            }
        }
        public ICell Create(Player? player, LinearTransform transform, bool placeable, bool drawGray)
        {
            return new Tile(player, transform, placeable, drawGray);
        }
        public ICell Place(IEnumerable<ICell> cells, Player player, bool placeable)
        {
            return new Tile(player, Transform, placeable, false);
        }
        public ICell DeepCopyPlacable(bool placeable)
        {
            return new Tile(Player, Transform, placeable, DrawGray);
        }
        public IEnumerable<Address> PathTo(ICell cell) => new List<Address>();
        public bool Contains(ICell cell) => cell.Equals(this);
    }
}