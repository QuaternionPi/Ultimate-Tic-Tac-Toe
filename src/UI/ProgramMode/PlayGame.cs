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
    private Func<LargeGrid<Grid<Tile>, Tile>> NewBoard;
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
        IProgramMode? previous,
        Player player1,
        Player player2,
        TimeSpan turnDelay,
        TimeSpan transitionTime,
        Func<LargeGrid<Grid<Tile>, Tile>> newBoard
    )
    {
        Previous = previous;
        NewBoard = newBoard;
        var board = newBoard();
        TurnDelay = turnDelay;
        TransitionTime = transitionTime;
        _game = new Game.Game(player1, player2, board, TurnDelay, TransitionTime);

        BoardEvaluator b1 = new(0, 0, 0, 100);
        BoardEvaluator b2 = new(0, 0, 10, 100);
        BoardEvaluator b3 = new(0, 10, 20, 100);
        BoardEvaluator b4 = new(10, 20, 30, 100);
        BoardEvaluator b5 = new(20, 30, 40, 100);
        BoardEvaluator b6 = new(30, 40, 50, 100);

        List<LargeBoardEvaluator> genomes = [
            new LargeBoardEvaluator(b1, b2, b3, 100),
            new LargeBoardEvaluator(b2, b3, b4, 100),
            new LargeBoardEvaluator(b3, b4, b5, 100),
            new LargeBoardEvaluator(b4, b5, b6, 100),
            new LargeBoardEvaluator(b5, b6, b1, 400),
            new LargeBoardEvaluator(b6, b1, b2, 400),
        ];
        Pool<LargeBoardEvaluator> pool = new(genomes, (eval1, eval2) =>
        {
            var p1 = new Bot(eval1.Evaluate, Player.Symbol.X, Color.RED, 0);
            var p2 = new Bot(eval2.Evaluate, Player.Symbol.O, Color.BLUE, 0);
            var game = new Game.Game(p1, p2, newBoard(), new TimeSpan(0), new TimeSpan(0));
            bool foundWinner = false;
            Player? winner = null;
            game.GameOver += (sender, player) => { winner = player; foundWinner = true; };
            game.Start();

            TimeSpan sleepTime = new(0, 0, 0, 0, 10);
            while (!foundWinner)
            {
                Thread.Sleep(sleepTime);
                game.Update();
            }
            return winner == null ? 0 : winner == p1 ? 1 : -1;
        }, new Random(1));
        for (int i = 0; i < 20; i++)
            pool.RunGeneration(1);

        Game.GameOver += GameOver;
        Game.Start();
    }
    protected void GameOver(Game.Game sender, Player? winner)
    {
        if (winner != null)
        {
            winner.Score += 1;
        }
        var board = NewBoard();
        Game = new Game.Game(sender.InactivePlayer, sender.ActivePlayer, board, TurnDelay, TransitionTime);
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