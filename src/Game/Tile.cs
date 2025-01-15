using System.Numerics;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe.Game;
public class Tile : ICell
{
    [JsonInclude]
    public Player.Player? Player { get; }
    public bool Placeable { get; }
    public Tile(Player.Player? player)
    {
        Player = player;
        Placeable = player == null;
    }
    public ICell Place(Player.Player? player)
    {
        return new Tile(player);
    }
}