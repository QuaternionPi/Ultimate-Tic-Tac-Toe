using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe
{
    namespace UI
    {
        public class BannerControler : IDrawable
        {
            public BannerControler(Player[] players)
            {
                Font = Graphics.Text.GetFontDefault();
                LinearTransform leftTransform = new(new Vector2(85, 85), 0, 3);
                LinearTransform rightTransform = new(new Vector2(815, 85), 0, 3);
                LeftBanner = new Banner(players[1], leftTransform, false);
                RightBanner = new Banner(players[0], rightTransform, true);
            }
            protected Banner LeftBanner;
            protected Banner RightBanner;
            protected readonly Font Font;
            public void Draw()
            {
                string message = "Ultimate Tic Tac Toe";
                float spacing = 3;
                float fontSize = 30;
                float messageWidth = Graphics.Text.MeasureTextEx(Font, message, fontSize, spacing).X;
                Graphics.Text.DrawTextEx(Font, message, new Vector2(450 - messageWidth / 2, 20), fontSize, spacing, Color.GRAY);
                LeftBanner.Draw();
                RightBanner.Draw();
            }
            public void Activate(Player player)
            {
                if (player == LeftBanner.Player)
                    LeftBanner = new Banner(
                        LeftBanner.Player,
                        LeftBanner.Transform,
                        true
                    );
                else
                    RightBanner = new Banner(
                        RightBanner.Player,
                        RightBanner.Transform,
                        true
                    );
            }
            public void Deactivate(Player player)
            {
                if (player == LeftBanner.Player)
                    LeftBanner = new Banner(
                        LeftBanner.Player,
                        LeftBanner.Transform,
                        false
                    );
                else
                    RightBanner = new Banner(
                        RightBanner.Player,
                        RightBanner.Transform,
                        false
                    );
            }
        }
    }
}