namespace UltimateTicTacToe.UI.ProgramMode;
public interface IProgramMode : IDrawable, IUpdatable, ITransitional
{
    public event Action<IProgramMode, IProgramMode>? SwitchTo;
    public IProgramMode? Previous { get; }
}