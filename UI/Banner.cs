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
        public class Banner : IDrawable
        {
            public Banner(Player player, LinearTransform transform, bool active, int score)
            {
                Player = player;
                Transform = transform;
                Active = active;
                Score = score;
                Font = GetFontDefault();
                Tile = new Tile(Player, Transform, false, 0);
                Tile.DrawGray = !Active;
            }
            public LinearTransform Transform { get; }
            public Player Player { get; }
            public bool Active { get; }
            protected Tile Tile { get; }
            public int Score { get; }
            protected Font Font { get; }
            public void Draw()
            {
                string message = Score.ToString();
                float spacing = 3;
                float fontSize = 80;
                float messageWidth = MeasureTextEx(Font, message, fontSize, spacing).X;
                Vector2 drawPosition = Transform.Position + new Vector2(-messageWidth / 2, 70);
                DrawTextEx(Font, message, drawPosition, fontSize, spacing, Color.LIGHTGRAY);
                Tile.Draw();
            }
        }
    }
}