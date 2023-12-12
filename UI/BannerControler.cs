using System.Numerics;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe
{
    namespace UI
    {
        /*
        Controls the position and activation of two banners; one for each team
        */
        public class BannerControler : IDrawable
        {
            public BannerControler(Player active, Player inactive)
            {
                Font = Graphics.Text.GetFontDefault();
                Transform2D rightTransform = new(new Vector2(815, 85), 0, 3);
                Transform2D leftTransform = new(new Vector2(85, 85), 0, 3);
                RightBanner = new Banner(active, rightTransform, true);
                LeftBanner = new Banner(inactive, leftTransform, false);
            }
            [JsonInclude]
            public Banner RightBanner { get; protected set; }
            [JsonInclude]
            public Banner LeftBanner { get; protected set; }
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