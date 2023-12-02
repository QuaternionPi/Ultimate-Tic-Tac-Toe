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
        LinearTransform Transform { get; }
        public Player? Player { get; }
        public bool Placeable { get; }
        public ICell Create(Player? player, LinearTransform transform, bool placeable, bool drawGray);
        public ICell Place(IEnumerable<Address> path, Player player, bool placeable, bool isRoot);
        public ICell DeepCopyPlacable(bool placeable);
        public delegate void ClickHandler(ICell cell, IEnumerable<Address> from, bool placeable);
        public event ClickHandler? Clicked;
    }
}