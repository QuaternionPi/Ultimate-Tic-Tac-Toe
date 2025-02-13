using System.Numerics;
using Raylib_cs;
using UltimateTicTacToe.Serialization;

namespace UltimateTicTacToe.UI.ProgramMode;
public class Load : IProgramMode
{
    public Load(IProgramMode previous)
    {
        var textColor = Color.GRAY;
        var backgroundColor = Color.LIGHTGRAY;
        var borderColor = backgroundColor;
        var size = new Vector2(300, 70);

        Buttons = [];
        var transform = new Transform2D(new Vector2(450, 100));
        var delta = new Vector2(0, 30);
        var saves = SaveManager.AllSaves();
        foreach (var save in saves)
        {
            var message = save.Name;
            var button = new Button(transform, size, message, textColor, backgroundColor, borderColor);
            button.Clicked += () =>
            {
                var game = save.Game;
                var playGame = new PlayGame(this, game);
                SwitchTo?.Invoke(previous, playGame);
            };
            Buttons.Add(button);
            transform = transform.Translate(delta);
        }
    }
    protected List<Button> Buttons;
    public bool InTransition { get; }
    public float TransitionValue { get; }
    public IProgramMode? Previous => null;
    public event Action<IProgramMode, IProgramMode>? SwitchTo;
    public void Draw()
    {
        Buttons.ForEach(button => button.Draw());
        Font font = Graphics.Text.GetFontDefault();
        string message = $"Ultimate Tic Tac Toe";
        float spacing = 3;
        float fontSize = 40;
        float messageWidth = Graphics.Text.MeasureTextEx(font, message, fontSize, spacing).X;
        Graphics.Text.DrawTextEx(font, message, new Vector2(450 - messageWidth / 2, 20), fontSize, spacing, Color.GRAY);
    }
    public void Update()
    {
        Buttons.ForEach(button => button.Update());
    }
}