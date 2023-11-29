using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;


namespace UltimateTicTacToe
{
    public class Banner : IDrawable
    {
        public Banner(Team team, LinearTransform transform, bool active, int score)
        {
            Team = team;
            Transform = transform;
            Active = active;
            Score = score;
            Font = GetFontDefault();
            _tile = new Tile(Team, Transform, false, !Active);
        }
        public void Draw()
        {
            string message = Score.ToString();
            float spacing = 3;
            float fontSize = 80;
            float messageWidth = MeasureTextEx(Font, message, fontSize, spacing).X;
            Vector2 drawPosition = Transform.Position + new Vector2(-messageWidth / 2, 70);
            DrawTextEx(Font, message, drawPosition, fontSize, spacing, Color.LIGHTGRAY);
            _tile.Draw();
        }
        public LinearTransform Transform { get; protected set; }
        public Team Team { get; }
        public bool Active { get; }
        protected Tile _tile;
        public int Score { get; }
        Font Font;
    }
}