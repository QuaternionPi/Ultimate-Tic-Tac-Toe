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
        public class BannerControler : IDrawable
        {
            public BannerControler(Player[] players)
            {
                Font = GetFontDefault();
                LinearTransform leftTransform = new(new Vector2(85, 85), 0, 3);
                LinearTransform rightTransform = new(new Vector2(815, 85), 0, 3);
                LeftBanner = new Banner(players[1], leftTransform, false, 0);
                RightBanner = new Banner(players[0], rightTransform, true, 0);
            }
            protected Banner LeftBanner;
            protected Banner RightBanner;
            protected readonly Font Font;
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
            public void Activate(Player player)
            {
                if (player == LeftBanner.Player)
                    LeftBanner = new Banner(
                        LeftBanner.Player,
                        LeftBanner.Transform,
                        true,
                        LeftBanner.Score
                    );
                else
                    RightBanner = new Banner(
                        RightBanner.Player,
                        RightBanner.Transform,
                        true,
                        RightBanner.Score
                    );
            }
            public void Deactivate(Player player)
            {
                if (player == LeftBanner.Player)
                    LeftBanner = new Banner(
                        LeftBanner.Player,
                        LeftBanner.Transform,
                        false,
                        LeftBanner.Score
                    );
                else
                    RightBanner = new Banner(
                        RightBanner.Player,
                        RightBanner.Transform,
                        false,
                        RightBanner.Score
                    );
            }
            public void AddPoints(Player player, int points)
            {
                if (LeftBanner.Player == player)
                {
                    LeftBanner.Score += points;
                }
                else if (RightBanner.Player == player)
                {
                    RightBanner.Score += points;
                }
                else
                {
                    throw new Exception("This player is not on either of the banners");
                }
            }
        }
    }
}