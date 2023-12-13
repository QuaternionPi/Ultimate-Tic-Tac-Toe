using System.Numerics;
using Raylib_cs;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToe
{
    namespace UI
    {
        namespace ProgramMode
        {
            public class QuickMenu : IProgramMode
            {
                public bool InTransition { get; }
                public float TransitionValue { get; }
                public event IProgramMode.SwitchToDel? SwitchTo;
                public IProgramMode? Previous { get; }
                public void Draw()
                {
                }
                public void Update()
                {
                }
            }
        }
    }
}