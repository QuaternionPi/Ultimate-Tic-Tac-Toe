using System.Numerics;
using Raylib_cs;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToe
{
    namespace UI
    {
        namespace ProgramMode
        {
            public class PlayGame : IProgramMode
            {
                public PlayGame(Player player1, Player player2)
                {

                    var position = new Vector2(450, 350);
                    var transform = new Transform2D(position, 0, 4);
                    var board = new Grid<Grid<Tile>>(null, transform, true);
                    _game = new Game.Game(player1, player2, board);
                    Game.GameOver += GameOver;
                }
                public bool InTransition { get; }
                public float TransitionValue { get; }
                public event IProgramMode.SwitchToDel? SwitchTo;
                public IProgramMode? Previous { get; }
                private Game.Game _game;
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
                protected void GameOver(Game.Game sender, Player? winner)
                {
                    if (winner != null)
                    {
                        winner.Score += 1;
                    }
                    var board = new Grid<Grid<Tile>>(null, sender.Board.Transform, true);
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
        }
    }
}