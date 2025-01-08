using System.Diagnostics;

namespace UltimateTicTacToe;
/*
Stores a position on a 3x3 grid
*/
public readonly struct Address
{
    public Address(int index)
    {
        Debug.Assert(index < 9 && index >= 0, $"Index {index} is not in grid");
        X = index / 3;
        Y = index % 3;
        XY = (X, Y);
        Index = index;
    }
    public int Index { get; }
    public int X { get; }
    public int Y { get; }
    public (int, int) XY { get; }
}