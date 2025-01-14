using System.Numerics;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe.Game;
public class Tile : ICell
{
    public Tile()
    {
        Player = null;
    }
    public Tile(Player.Player? player)
    {
        Player = player;
    }
    [JsonInclude]
    public Player.Player? Player { get; }
    public ICell Place(Player.Player? player)
    {
        return new Tile(player);
    }
}