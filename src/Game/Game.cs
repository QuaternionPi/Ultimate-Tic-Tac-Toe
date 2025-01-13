using System.Numerics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;
public class Game : IDrawable, IUpdatable
{
    public Game(Player.Player active, Player.Player inactive, LargeGrid<Grid<Tile>, Tile> board)
    {
        Board = board;
        var position = new Vector2(450, 350);
        var transform = new Transform2D(position, 0, 4);
        BoardUI = new(board, transform);
        ActivePlayer = active;
        InactivePlayer = inactive;
        ActivePlayer.PlayTurn += HandlePlayerTurn;
        InactivePlayer.PlayTurn += HandlePlayerTurn;
        BannerController = new UI.BannerController(active, inactive);


        TimeSpan delay = new(0, 0, 0, 0, 300);
        Thread thread = new(() => DelayedPlayerStart(delay));
        thread.Start();
        //ActivePlayer.BeginTurn(Board, InactivePlayer);
    }
    [JsonInclude]
    public LargeGrid<Grid<Tile>, Tile> Board { get; protected set; }
    public UI.LargeBoard<Grid<Tile>, Tile> BoardUI { get; protected set; }
    [JsonInclude]
    public Player.Player ActivePlayer { get; protected set; }
    [JsonInclude]
    public Player.Player InactivePlayer { get; protected set; }
    protected bool ChangePlayer;
    [JsonInclude]
    public UI.BannerController BannerController { get; protected set; }
    [JsonInclude]
    public int TurnNumber { get; protected set; }
    public delegate void GameOverDel(Game sender, Player.Player? winner);
    public event GameOverDel? GameOver;
    protected void NextPlayer()
    {
        Player.Player temp;
        ActivePlayer.EndTurn();

        temp = ActivePlayer;
        ActivePlayer = InactivePlayer;
        InactivePlayer = temp;

        TimeSpan delay = new(0, 0, 0, 0, 100);
        Thread thread = new(() => DelayedPlayerStart(delay));
        thread.Start();
        //ActivePlayer.BeginTurn(Board, InactivePlayer);
        TurnNumber++;
    }
    protected void HandlePlayerTurn(Player.Player player, ILargeBoard<Grid<Tile>, Tile> board, Grid<Tile> grid, Tile tile)
    {
        if (player != ActivePlayer)
        {
            Console.WriteLine($"Not player {player}'s turn");
            return;
        }
        var position = new Vector2(450, 350);
        var transform = new Transform2D(position, 0, 4);
        Board = (LargeGrid<Grid<Tile>, Tile>)Board.Place(grid, tile, ActivePlayer);
        BoardUI = new UI.LargeBoard<Grid<Tile>, Tile>(board, transform);
        ChangePlayer = true;
    }
    protected void DelayedPlayerStart(TimeSpan delay)
    {
        Thread.Sleep(delay);
        ActivePlayer.BeginTurn(Board, InactivePlayer);
    }
    public void Draw()
    {
        BoardUI.Draw();
        BannerController.Draw();
    }
    public void Update()
    {
        BoardUI.Update();
        if (ChangePlayer && Board.AnyPlaceable)
        {
            ChangePlayer = false;
            NextPlayer();
        }
        ActivePlayer.Update();
        // Board currently in a transition
        if (BoardUI.TransitionValue != 0)
        {
            return;
        }
        // Board won by a player or the board cannot be placed on
        if (Board.Player != null || Board.AnyPlaceable == false)
        {
            ActivePlayer.PlayTurn -= HandlePlayerTurn;
            InactivePlayer.PlayTurn -= HandlePlayerTurn;
            GameOver?.Invoke(this, Board.Player);
            return;
        }
        // Toggle players
        BannerController.Activate(ActivePlayer);
        BannerController.Deactivate(InactivePlayer);
    }
}