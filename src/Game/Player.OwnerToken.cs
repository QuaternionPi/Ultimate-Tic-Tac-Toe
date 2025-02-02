namespace UltimateTicTacToe.Game;
public abstract partial class Player : IEquatable<Player.Token>
{
    private readonly Token _token;
    public Token GetToken() => _token;
    public bool Equals(Token other)
    {
        return _token.Equals(other);
    }
    public readonly struct Token(int id)
    {
        private readonly int _id = id;
    }
}