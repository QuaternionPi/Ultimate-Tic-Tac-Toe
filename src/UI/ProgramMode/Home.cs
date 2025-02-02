using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe.UI.ProgramMode;
public class Home : IProgramMode
{
    public Home()
    {
        var continueTransform = new Transform2D(new Vector2(170, 190));
        var newGameTransform = new Transform2D(new Vector2(170, 290));
        var loadTransform = new Transform2D(new Vector2(170, 390));
        var settingsTransform = new Transform2D(new Vector2(170, 490));
        var exitTransform = new Transform2D(new Vector2(170, 590));

        var textColor = Color.GRAY;
        var backgroundColor = Color.LIGHTGRAY;
        var borderColor = backgroundColor;
        var size = new Vector2(300, 70);

        var continueButton = new Button(continueTransform, size, "Continue", textColor, backgroundColor, borderColor);
        var newGameButton = new Button(newGameTransform, size, "New Game", textColor, backgroundColor, borderColor);
        var loadButton = new Button(loadTransform, size, "Load", textColor, backgroundColor, borderColor);
        var settingsButton = new Button(settingsTransform, size, "Settings", textColor, backgroundColor, borderColor);
        var exitButton = new Button(exitTransform, size, "Exit", textColor, backgroundColor, borderColor);

        Buttons = [continueButton, newGameButton, loadButton, settingsButton, exitButton];

        newGameButton.Clicked += () =>
        {
            var setup = new Setup();
            SwitchTo?.Invoke(this, setup);
        };
        exitButton.Clicked += () =>
        {
            var confirm = new Confirm(this, "Are you certain you wish to exit?", () => Program.Exit(0));
            SwitchTo?.Invoke(this, confirm);
        };
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