using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe.UI;
/*
Displays a team's symbol and score
*/
public class Banner : IDrawable
{
    public Banner(Player.Player player, Transform2D transform, bool active)
    {
        Player = player;
        Transform = transform;
        Active = active;
        Font = Graphics.Text.GetFontDefault();
        Tile = new Game.Tile(Player, Transform, 0);
    }
    public Transform2D Transform { get; }
    public Player.Player Player { get; }
    public bool Active { get; }
    protected Game.Tile Tile { get; }
    protected Font Font { get; }
    public void Draw()
    {
        if (Active == false)
        {
            Graphics.Draw.OverrideDrawColor = true;
            Graphics.Draw.OverrideColor = Color.LIGHTGRAY;
        }
        Tile.Draw();
        Graphics.Draw.OverrideDrawColor = false;
        DrawScore();
    }
    protected void DrawScore()
    {
        string message = Player.Score.ToString();
        float spacing = 3;
        float fontSize = 80;
        float messageWidth = Graphics.Text.MeasureTextEx(Font, message, fontSize, spacing).X;
        Vector2 drawPosition = Transform.Position + new Vector2(-messageWidth / 2, 70);
        Graphics.Text.DrawTextEx(Font, message, drawPosition, fontSize, spacing, Color.LIGHTGRAY);
    }
}