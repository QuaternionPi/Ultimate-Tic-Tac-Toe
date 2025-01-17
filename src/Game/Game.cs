using System.Numerics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;
public class Game : IDrawable, IUpdatable
{
    [JsonInclude]
    private LargeGrid<Grid<Tile>, Tile> Board { get; set; }
    private UI.LargeBoard<Grid<Tile>, Tile> BoardUI { get; set; }
    [JsonInclude]
    public Player ActivePlayer { get; protected set; }
    [JsonInclude]
    public Player InactivePlayer { get; protected set; }
    private bool ChangePlayer;
    [JsonInclude]
    private UI.BannerController BannerController { get; set; }
    [JsonInclude]
    public int TurnNumber { get; protected set; }
    public event Action<Game, Player?>? GameOver;
    public Game(Player active, Player inactive, LargeGrid<Grid<Tile>, Tile> board)
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
    }
    protected void NextPlayer()
    {
        Player temp;
        ActivePlayer.EndTurn();

        temp = ActivePlayer;
        ActivePlayer = InactivePlayer;
        InactivePlayer = temp;

        TimeSpan delay = new(0, 0, 0, 0, 100);
        Thread thread = new(() => DelayedPlayerStart(delay));
        thread.Start();
        TurnNumber++;
    }
    protected void HandlePlayerTurn(Player player, ILargeBoard<Grid<Tile>, Tile> board, int index, int innerIndex)
    {
        if (player != ActivePlayer)
        {
            Console.WriteLine($"Not player {player}'s turn");
            return;
        }
        Board = (LargeGrid<Grid<Tile>, Tile>)Board.Place(ActivePlayer, index, innerIndex);
        BoardUI.UpdateLargeBoard(Board);
        ChangePlayer = true;
    }
    protected void DelayedPlayerStart(TimeSpan delay)
    {
        Thread.Sleep(delay);
        ActivePlayer.BeginTurn(Board, BoardUI, InactivePlayer);
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
        if (BoardUI.InTransition)
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