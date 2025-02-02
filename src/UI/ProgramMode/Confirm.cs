using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe.UI.ProgramMode;
public class Confirm : IProgramMode
{
    public Confirm(IProgramMode previous, string message, Action action)
    {
        Previous = previous;
        Message = message;
        var confirmTransform = new Transform2D(new Vector2(170, 490));
        var cancelTransform = new Transform2D(new Vector2(170, 590));

        var textColor = Color.GRAY;
        var backgroundColor = Color.LIGHTGRAY;
        var borderColor = backgroundColor;
        var size = new Vector2(300, 70);

        var confirmButton = new Button(confirmTransform, size, "Confirm", textColor, backgroundColor, borderColor);
        var cancelButton = new Button(cancelTransform, size, "Cancel", textColor, backgroundColor, borderColor);

        Buttons = [confirmButton, cancelButton];

        confirmButton.Clicked += () =>
        {
            action();
        };
        cancelButton.Clicked += () =>
        {
            SwitchTo?.Invoke(this, previous);
        };
    }
    protected List<Button> Buttons;
    protected string Message { get; }
    public bool InTransition { get; }
    public float TransitionValue { get; }
    public IProgramMode? Previous { get; protected set; }
    public event Action<IProgramMode, IProgramMode>? SwitchTo;
    public void Draw()
    {
        Buttons.ForEach(button => button.Draw());
        Font font = Graphics.Text.GetFontDefault();
        float spacing = 3;
        float fontSize = 40;
        float messageWidth = Graphics.Text.MeasureTextEx(font, Message, fontSize, spacing).X;
        Graphics.Text.DrawTextEx(font, Message, new Vector2(450 - messageWidth / 2, 20), fontSize, spacing, Color.GRAY);
    }
    public void Update()
    {
        Buttons.ForEach(button => button.Update());
    }
}