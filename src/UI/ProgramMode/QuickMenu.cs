namespace UltimateTicTacToe.UI.ProgramMode;

public class QuickMenu : IProgramMode
{
    public bool InTransition { get; }
    public float TransitionValue { get; }
    public event Action<IProgramMode, IProgramMode>? SwitchTo;
    public IProgramMode? Previous { get; }
    public void Draw() { }
    public void Update() { }
}