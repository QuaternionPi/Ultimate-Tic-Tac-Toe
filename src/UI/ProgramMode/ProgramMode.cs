namespace UltimateTicTacToe.UI.ProgramMode;
public interface IProgramMode
{
    public IProgramMode? Previous { get; }
    public bool InTransition { get; }
    public float TransitionValue { get; }
    public event Action<IProgramMode, IProgramMode>? SwitchTo;
    public void Update();
    public void Draw();
}