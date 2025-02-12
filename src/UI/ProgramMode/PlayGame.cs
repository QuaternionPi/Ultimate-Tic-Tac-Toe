using System.Numerics;
using Raylib_cs;
using UltimateTicTacToe.Game;
using UltimateTicTacToe.Genetics;

namespace UltimateTicTacToe.UI.ProgramMode;
public class PlayGame : IProgramMode
{
    public bool InTransition { get; }
    public float TransitionValue { get; }
    public event Action<IProgramMode, IProgramMode>? SwitchTo;
    public IProgramMode? Previous { get; }
    private Game.Game _game;
    protected Game.Game Game
    {
        get { return _game; }
        set
        {
            Game.GameOver -= GameOver;
            _game = value;
            Game.GameOver += GameOver;
        }
    }
    private TimeSpan TurnDelay { get; }
    private TimeSpan TransitionTime { get; }
    public PlayGame(
        IProgramMode? previous, Game.Game game
    )
    {
        Previous = previous;
        _game = game;
        Game.GameOver += GameOver;
        Game.Start();
    }
    protected void GameOver(Game.Game sender, Player? winner)
    {
        if (winner != null)
        {
            winner.Score += 1;
        }
        Game.Reset();
        Game.Start();
    }
    public void Draw()
    {
        Font font = Graphics.Text.GetFontDefault();
        string message = $"Turn {Game.TurnNumber}";
        float spacing = 3;
        float fontSize = 30;
        float messageWidth = Graphics.Text.MeasureTextEx(font, message, fontSize, spacing).X;
        Graphics.Text.DrawTextEx(font, message, new Vector2(450 - messageWidth / 2, 20), fontSize, spacing, Color.GRAY);
        Game.Draw();
    }
    public void Update()
    {
        Game.Update();
    }
}