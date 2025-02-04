using System.Numerics;
using Raylib_cs;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToe.UI;
/*
Displays a team's symbol and score
*/
public class Banner
{
    public Banner(Game.Player player, Transform2D transform, bool active)
    {
        Player = player;
        Transform = transform;
        Active = active;
        Font = Graphics.Text.GetFontDefault();
        Cell = new Cell(Player.GetToken(), Transform, 0);
    }
    public Transform2D Transform { get; }
    public Game.Player Player { get; }
    public bool Active { get; }
    protected Cell Cell { get; }
    protected Font Font { get; }
    public void Draw()
    {
        if (Active == false)
        {
            Graphics.Draw.OverrideDrawColor = true;
            Graphics.Draw.OverrideColor = Color.LIGHTGRAY;
        }
        Cell.Draw();
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