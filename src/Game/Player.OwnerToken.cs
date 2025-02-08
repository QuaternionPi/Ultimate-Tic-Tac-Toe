using Raylib_cs;

namespace UltimateTicTacToe.Game;
public abstract partial class Player
{
    public Token GetToken() => new(Shape, Color);
    public record Token(Symbol Symbol, Color Color) { }
}