using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;
public class Game
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
    private TimeSpan TurnDelay { get; set; }
    public bool InProgress { get; protected set; }
    public event Action<Game, Player?>? GameOver;
    public Game(Player active, Player inactive, LargeGrid<Grid<Tile>, Tile> board, TimeSpan turnDelay)
    {
        ActivePlayer = active;
        InactivePlayer = inactive;
        Board = board;
        TurnDelay = turnDelay;
        InProgress = false;
        var position = new Vector2(450, 350);
        var transform = new Transform2D(position, 0, 4);
        BoardUI = new(board, transform);
        ActivePlayer.PlayTurn += HandlePlayerTurn;
        InactivePlayer.PlayTurn += HandlePlayerTurn;
        BannerController = new UI.BannerController(active, inactive);
    }
    public void Start()
    {
        Debug.Assert(!InProgress, "This game is already in progress; it cannot be started");
        InProgress = true;
        DelayedPlayTurn();
    }
    protected void NextPlayer()
    {
        Player temp;
        ActivePlayer.EndTurn();

        temp = ActivePlayer;
        ActivePlayer = InactivePlayer;
        InactivePlayer = temp;

        DelayedPlayTurn();
        TurnNumber++;
    }
    protected void HandlePlayerTurn(Player player, ILargeBoard<Grid<Tile>, Tile> board, int index, int innerIndex)
    {
        if (player != ActivePlayer)
        {
            Console.WriteLine($"Not player {player}'s turn");
            return;
        }
        Board = Board.Place(ActivePlayer, index, innerIndex);
        BoardUI.UpdateLargeBoard(Board);
        ChangePlayer = true;
    }
    protected void DelayedPlayTurn()
    {
        new Thread(() =>
        {
            Thread.Sleep(TurnDelay);
            ActivePlayer.BeginTurn(Board, BoardUI, InactivePlayer);
        }).Start();
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
            ActivePlayer.EndTurn();
            GameOver?.Invoke(this, Board.Player);
            return;
        }
        // Toggle players
        BannerController.Activate(ActivePlayer);
        BannerController.Deactivate(InactivePlayer);
    }
}