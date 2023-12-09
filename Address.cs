namespace UltimateTicTacToe
{
    public readonly struct Address
    {
        public Address(int x, int y)
        {
            if (x > 2 | x < 0 | y > 2 | y < 0)
            {
                throw new IndexOutOfRangeException("Position not in grid");
            }
            X = x;
            Y = y;
        }
        public int X { get; }
        public int Y { get; }
        public (int, int) XY { get { return (X, Y); } }
    }
}