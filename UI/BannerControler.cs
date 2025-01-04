using System.Numerics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.UI;
/*
Controls the position and activation of two banners; one for each team
*/
public class BannerController : IDrawable
{
    public BannerController(Player active, Player inactive)
    {
        Transform2D rightTransform = new(new Vector2(815, 85), 0, 3);
        Transform2D leftTransform = new(new Vector2(85, 85), 0, 3);
        RightBanner = new Banner(active, rightTransform, true);
        LeftBanner = new Banner(inactive, leftTransform, false);
    }
    [JsonInclude]
    public Banner RightBanner { get; protected set; }
    [JsonInclude]
    public Banner LeftBanner { get; protected set; }
    public void Draw()
    {
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