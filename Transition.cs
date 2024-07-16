namespace UltimateTicTacToe;
public interface ITransitionable
{
    public bool InTransition { get; }
    public float TransitionValue { get; }
}