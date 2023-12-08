using System.Numerics;
using Raylib_cs;

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
                Font = Graphics.Text.GetFontDefault();
                Tile = new Game.Tile(Player, Transform, false, 0);
                Tile.DrawGray = !Active;
            }
            public LinearTransform Transform { get; }
            public Player Player { get; }
            public bool Active { get; }
            protected Game.Tile Tile { get; }
            public int Score { get; set; }
            protected Font Font { get; }
            public void Draw()
            {
                Tile.Draw();
                DrawScore();
            }
            protected void DrawScore()
            {
                string message = Score.ToString();
                float spacing = 3;
                float fontSize = 80;
                float messageWidth = Graphics.Text.MeasureTextEx(Font, message, fontSize, spacing).X;
                Vector2 drawPosition = Transform.Position + new Vector2(-messageWidth / 2, 70);
                Graphics.Text.DrawTextEx(Font, message, drawPosition, fontSize, spacing, Color.LIGHTGRAY);
            }
        }
    }
}