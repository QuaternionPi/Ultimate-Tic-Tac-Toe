using System.Numerics;
using Raylib_cs;
using UltimateTicTacToe.Serialization;

namespace UltimateTicTacToe.UI.ProgramMode;

public class Load : IProgramMode
{
    protected bool AnySavesFound { get; }
    public Load(IProgramMode previous)
    {
        Previous = previous;
        var textColor = Color.GRAY;
        var backgroundColor = Color.LIGHTGRAY;
        var borderColor = backgroundColor;
        var size = new Vector2(300, 70);

        Buttons = [];
        var transform = new Transform2D(new Vector2(450, 110));
        var delta = new Vector2(0, 30);
        var saves = SaveManager.AllSaves();
        AnySavesFound = saves.Count != 0;
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
    public IProgramMode Previous { get; }
    public event Action<IProgramMode, IProgramMode>? SwitchTo;
    public void Draw()
    {
        Font font = Graphics.Text.GetFontDefault();
        string message = "Load game";
        float spacing = 3;
        float fontSize = 40;
        float messageWidth = Graphics.Text.MeasureTextEx(font, message, fontSize, spacing).X;
        Graphics.Text.DrawTextEx(font, message, new Vector2(450 - messageWidth / 2, 20), fontSize, spacing, Color.GRAY);

        if (!AnySavesFound)
        {
            fontSize = 30;
            message = "No saves found";
            messageWidth = Graphics.Text.MeasureTextEx(font, message, fontSize, spacing).X;
            Graphics.Text.DrawTextEx(font, message, new Vector2(450 - messageWidth / 2, 80), fontSize, spacing, Color.GRAY);
        }

        Buttons.ForEach(button => button.Draw());
    }
    public void Update()
    {
        if (Keyboard.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
        {
            SwitchTo?.Invoke(this, Previous);
        }
        Buttons.ForEach(button => button.Update());
    }
}