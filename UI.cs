using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;


namespace UltimateTicTacToe
{
    class UI : IDrawable
    {
        protected class Banner : IDrawable, ITransform
        {
            public Banner(Team team, Vector2 position, bool active, int score)
            {
                Team = team;
                _font = GetFontDefault();
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
                float messageWidth = MeasureTextEx(_font, message, fontSize, spacing).X;
                Vector2 drawPosition = Position + new Vector2(-messageWidth / 2 + 75, 170);
                DrawTextEx(_font, message, drawPosition, fontSize, spacing, Color.LIGHTGRAY);
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
            Font _font;
        }
        public UI(Team[] teams)
        {
            _font = GetFontDefault();
            _leftBanner = new Banner(teams[1], new Vector2(0, 0), false, 0);
            _rightBanner = new Banner(teams[0], new Vector2(750, 0), true, 0);
        }
        public void Activate(Team team)
        {
            _leftBanner = new Banner(
                _leftBanner.Team,
                _leftBanner.Position,
                _leftBanner.Team == team,
                _leftBanner.Score
            );
            _rightBanner = new Banner(
                _rightBanner.Team,
                _rightBanner.Position,
                _rightBanner.Team == team,
                _rightBanner.Score
            );
        }
        public void AddPoints(Team team, int points)
        {
            Banner banner;
            if (_leftBanner.Team == team)
            {
                banner = _leftBanner;
            }
            else if (_rightBanner.Team == team)
            {
                banner = _rightBanner;
            }
            else
            {
                throw new Exception("This team is not on either of the banners");
            }
            Vector2 position = banner.Position;
            bool active = banner.Active;
            int score = banner.Score + points;
            banner = new Banner(team, position, active, score);
            if (_leftBanner.Team == team)
            {
                _leftBanner = banner;
            }
            else if (_rightBanner.Team == team)
            {
                _rightBanner = banner;
            }
        }
        public void Draw()
        {
            string message = "Ultimate Tic Tac Toe";
            float spacing = 3;
            float fontSize = 30;
            float messageWidth = MeasureTextEx(_font, message, fontSize, spacing).X;
            DrawTextEx(_font, message, new Vector2(450 - messageWidth / 2, 20), fontSize, spacing, Color.GRAY);
            _leftBanner.Draw();
            _rightBanner.Draw();
        }
        Banner _leftBanner;
        Banner _rightBanner;
        Font _font;
    }
}