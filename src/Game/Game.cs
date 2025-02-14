using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;
public class Game
{
    [JsonInclude]
    public Player Active { get; protected set; }
    [JsonInclude]
    public Player Inactive { get; protected set; }
    [JsonInclude]
    private LargeGrid<Grid<Tile>, Tile> Board { get; set; }
    [JsonInclude]
    private LargeGrid<Grid<Tile>, Tile> ResetBoard { get; }
    [JsonIgnore]
    private bool ChangePlayer;
    [JsonIgnore]
    public bool InProgress { get; protected set; }
    [JsonInclude]
    public int TurnNumber { get; protected set; }
    [JsonInclude]
    private TimeSpan TurnDelay { get; }
    [JsonInclude]
    private TimeSpan TransitionTime { get; }
    [JsonIgnore]
    private UI.LargeBoard<Grid<Tile>, Tile> BoardUI { get; set; }
    [JsonIgnore]
    private UI.BannerController BannerController { get; }
    [JsonInclude]
    public bool UpdateUI { get; }
    public event Action<Game, Player?>? GameOver;
    [JsonConstructor]
    public Game
    (
        int turnNumber,
        Player active,
        Player inactive,
        LargeGrid<Grid<Tile>, Tile> board,
        LargeGrid<Grid<Tile>, Tile> resetBoard,
        TimeSpan turnDelay,
        TimeSpan transitionTime,
        bool updateUI = true
    )
    {
        TurnNumber = turnNumber;
        Active = active;
        Inactive = inactive;
        Board = board;
        ResetBoard = resetBoard;
        TurnDelay = turnDelay;
        TransitionTime = transitionTime;
        UpdateUI = updateUI;
        InProgress = false;
        var position = new Vector2(450, 350);
        var transform = new Transform2D(position, 0, 4);
        BoardUI = new(board, transform, transitionTime);
        BannerController = new UI.BannerController(active, inactive);
    }
    public void Start()
    {
        Debug.Assert(!InProgress, "This game is already in progress; it cannot be started");
        InProgress = true;
        Active.PlayTurn += HandlePlayerTurn;
        Inactive.PlayTurn += HandlePlayerTurn;
        DelayedPlayTurn();
    }
    public void Stop()
    {
        InProgress = false;
        Active.PlayTurn -= HandlePlayerTurn;
        Inactive.PlayTurn -= HandlePlayerTurn;
    }
    public void Reset()
    {
        TurnNumber = 0;
        InProgress = false;
        Board = ResetBoard;
        var position = new Vector2(450, 350);
        var transform = new Transform2D(position, 0, 4);
        BoardUI = new(Board, transform, TransitionTime);
    }
    protected void NextPlayer()
    {
        Active.EndTurn();
        (Inactive, Active) = (Active, Inactive);
        DelayedPlayTurn();
        TurnNumber++;
    }
    protected void HandlePlayerTurn(Player player, ILargeBoard<Grid<Tile>, Tile> board, (int, int) move)
    {
        if (board != Board)
        {
            return;
        }
        if (player != Active)
        {
            Console.WriteLine($"Not player {player}'s turn");
            return;
        }
        Board = Board.Place(Active.GetToken(), move);
        if (UpdateUI)
        {
            BoardUI.UpdateLargeBoard(Board);
        }
        ChangePlayer = true;
    }
    protected void DelayedPlayTurn()
    {
        new Thread(() =>
        {
            Thread.Sleep(TurnDelay);
            Active.BeginTurn(Board, BoardUI, Inactive);
        }).Start();
    }
    public void Draw()
    {
        BoardUI.Draw();
        BannerController.Draw();
    }
    public void Update()
    {
        if (ChangePlayer && Board.AnyPlaceable)
        {
            ChangePlayer = false;
            NextPlayer();
        }
        Active.Update();
        if (UpdateUI)
        {
            BoardUI.Update();
            // Board currently in a transition
            if (BoardUI.InTransition)
            {
                return;
            }
        }
        // Board won by a player or the board cannot be placed on
        var winner = Board.Winner;
        if (winner is not null || Board.AnyPlaceable == false)
        {
            Active.PlayTurn -= HandlePlayerTurn;
            Inactive.PlayTurn -= HandlePlayerTurn;
            Active.EndTurn();
            if (Active.GetToken().Equals(winner))
            {
                GameOver?.Invoke(this, Active);
            }
            else if (Inactive.GetToken().Equals(winner))
            {
                GameOver?.Invoke(this, Inactive);
            }
            else
            {
                GameOver?.Invoke(this, null);
            }
            return;
        }
        if (UpdateUI)
        {
            // Toggle players
            BannerController.Activate(Active);
            BannerController.Deactivate(Inactive);
        }
    }
}