using Raylib_cs;

namespace UltimateTicTacToe.Game;
public abstract partial class Player
{
    private readonly Token _token;
    public Token GetToken() => _token;
    public record Token(Symbol Symbol, Color Color) { }
}