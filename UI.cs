using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;


namespace UltimateTicTacToe
{
    public class UI : IDrawable
    {
        protected class Banner : IDrawable, ITransform
        {
            public Banner(Team team, Vector2 position, bool active, int score)
            {
                Team = team;
                Font = GetFontDefault();
                Position = position;
                LinearTransform transform = new(Position + new Vector2(75, 75), 0, 3);
                _tile = new Tile(team, transform, false, !active);
                Active = active;
                Score = score;
            }
            public void Draw()
            {
                string message = Score.ToString();
                float spacing = 3;
                float fontSize = 80;
                float messageWidth = MeasureTextEx(Font, message, fontSize, spacing).X;
                Vector2 drawPosition = Position + new Vector2(-messageWidth / 2 + 75, 170);
                DrawTextEx(Font, message, drawPosition, fontSize, spacing, Color.LIGHTGRAY);
                _tile.Draw();
            }
            public Vector2 Position
            {
                get; protected set;
            }
            public float Rotation
            {
                get; protected set;
            }
            public float Scale
            {
                get; protected set;
            }
            public Team Team { get; }
            public bool Active { get; }
            protected Tile _tile;
            public int Score { get; }
            Font Font;
        }
        public UI(Team[] teams)
        {
            Font = GetFontDefault();
            LeftBanner = new Banner(teams[1], new Vector2(0, 0), false, 0);
            RightBanner = new Banner(teams[0], new Vector2(750, 0), true, 0);
        }
        public void Activate(Team team)
        {
            LeftBanner = new Banner(
                LeftBanner.Team,
                LeftBanner.Position,
                LeftBanner.Team == team,
                LeftBanner.Score
            );
            RightBanner = new Banner(
                RightBanner.Team,
                RightBanner.Position,
                RightBanner.Team == team,
                RightBanner.Score
            );
        }
        public void AddPoints(Team team, int points)
        {
            Banner banner;
            if (LeftBanner.Team == team)
            {
                banner = LeftBanner;
            }
            else if (RightBanner.Team == team)
            {
                banner = RightBanner;
            }
            else
            {
                throw new Exception("This team is not on either of the banners");
            }
            Vector2 position = banner.Position;
            bool active = banner.Active;
            int score = banner.Score + points;
            banner = new Banner(team, position, active, score);
            if (LeftBanner.Team == team)
            {
                LeftBanner = banner;
            }
            else if (RightBanner.Team == team)
            {
                RightBanner = banner;
            }
        }
        public void Draw()
        {
            string message = "Ultimate Tic Tac Toe";
            float spacing = 3;
            float fontSize = 30;
            float messageWidth = MeasureTextEx(Font, message, fontSize, spacing).X;
            DrawTextEx(Font, message, new Vector2(450 - messageWidth / 2, 20), fontSize, spacing, Color.GRAY);
            LeftBanner.Draw();
            RightBanner.Draw();
        }
        protected Banner LeftBanner;
        protected Banner RightBanner;
        protected readonly Font Font;
    }
}