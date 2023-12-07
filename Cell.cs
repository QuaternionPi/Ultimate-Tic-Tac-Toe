using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public interface ICell : IDrawable, IUpdateable, ITransitionable
    {
        LinearTransform Transform { get; }
        public Player? Player { get; }
        public bool Placeable { get; }
        public ICell Create(Player? player, LinearTransform transform, bool placeable);
        public ICell Place(IEnumerable<ICell> cells, Player player, bool placeable);
        public ICell DeepCopyPlacable(bool placeable);
        public IEnumerable<Address> PathTo(ICell cell);
        public bool Contains(ICell cell);
        public delegate void ClickHandler(IEnumerable<ICell> cells);
        public event ClickHandler? Clicked;
    }
}