using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public interface ICell : IDrawable, IUpdateable
    {
        public ICell Create(Team? team, LinearTransform transform, bool placeable, bool drawGray);
        public ICell Place(IEnumerable<Address> path, Team team, bool placeable, bool isRoot);
        public ICell DeepCopyPlacable(bool placeable);
        LinearTransform Transform { get; }
        public Team? Team { get; }
        public bool Placeable { get; }
        public delegate void ClickHandler(ICell cell, IEnumerable<Address> from, bool placeable);
        public event ClickHandler? Clicked;
    }
}