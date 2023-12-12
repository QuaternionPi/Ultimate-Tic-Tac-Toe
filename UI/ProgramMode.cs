using System.Numerics;
using Raylib_cs;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToe
{
    namespace UI
    {
        namespace ProgramMode
        {
            public interface IProgramMode : IDrawable, IUpdateable, ITransitionable
            {
                public event SwitchToDel? SwitchTo;
                public delegate void SwitchToDel(IProgramMode sender, IProgramMode switchTo);
                public IProgramMode? Previous { get; }
            }
        }
    }
}