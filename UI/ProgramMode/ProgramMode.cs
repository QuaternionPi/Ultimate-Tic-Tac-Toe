namespace UltimateTicTacToe.UI.ProgramMode;
public interface IProgramMode : IDrawable, IUpdateable, ITransitionable
{
    public event SwitchToDel? SwitchTo;
    public delegate void SwitchToDel(IProgramMode sender, IProgramMode switchTo);
    public IProgramMode? Previous { get; }
}