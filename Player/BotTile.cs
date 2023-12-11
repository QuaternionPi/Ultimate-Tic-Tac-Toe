using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe
{
    public partial class Bot
    {
        public class Tile : ICell
        {
            public Tile()
            {
                Player = null;
                Placeable = false;
            }
            public Tile(Player? player, bool placeable)
            {
                Player = player;
                Placeable = placeable && Player == null;
            }
            public Player? Player { get; }
            public bool Placeable { get; }
            public ICell Create(Player? player, Transform2D transform, bool placeable)
            {
                return new Tile(player, placeable);
            }
            public ICell Place(IEnumerable<ICell> cells, Player player, bool placeable)
            {
                return new Tile(player, placeable);
            }
            public ICell DeepCopyPlacable(bool placeable)
            {
                return new Tile(Player, placeable);
            }
            public List<Address> PathTo(ICell cell) => new List<Address>();
            public bool Contains(ICell cell) => cell.Equals(this);
        }
    }
}