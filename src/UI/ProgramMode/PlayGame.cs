using System.Numerics;
using Raylib_cs;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToe.UI.ProgramMode;
public class PlayGame : IProgramMode
{
    public PlayGame(IProgramMode? previous, Player.Player player1, Player.Player player2, Func<LargeGrid<Grid<Tile>, Tile>> newBoard)
    {
        Previous = previous;
        NewBoard = newBoard;
        var board = newBoard();
        _game = new Game.Game(player1, player2, board);
        Game.GameOver += GameOver;
    }
    public bool InTransition { get; }
    public float TransitionValue { get; }
    public event Action<IProgramMode, IProgramMode>? SwitchTo;
    public IProgramMode? Previous { get; }
    private Game.Game _game;
    private Func<LargeGrid<Grid<Tile>, Tile>> NewBoard;
    protected Game.Game Game
    {
        get
        {
            return _game;
        }
        set
        {
            Game.GameOver -= GameOver;
            _game = value;
            Game.GameOver += GameOver;
        }
    }
    protected void GameOver(Game.Game sender, Player.Player? winner)
    {
        if (winner != null)
        {
            winner.Score += 1;
        }
        var board = NewBoard();
        Game = new Game.Game(sender.InactivePlayer, sender.ActivePlayer, board);
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