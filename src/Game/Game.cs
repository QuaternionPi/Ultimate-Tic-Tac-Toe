using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;
public class Game
{
    [JsonInclude]
    private LargeGrid<Grid<Tile>, Tile> Board { get; set; }
    [JsonIgnore]
    private UI.LargeBoard<Grid<Tile>, Tile> BoardUI { get; set; }
    [JsonInclude]
    public Player Active { get; protected set; }
    [JsonInclude]
    public Player Inactive { get; protected set; }
    [JsonIgnore]
    private bool ChangePlayer;
    [JsonIgnore]
    private UI.BannerController BannerController { get; set; }
    [JsonInclude]
    public int TurnNumber { get; protected set; }
    [JsonInclude]
    private TimeSpan TurnDelay { get; set; }
    [JsonInclude]
    private TimeSpan TransitionTime { get; set; }
    [JsonIgnore]
    public bool InProgress { get; protected set; }
    public event Action<Game, Player?>? GameOver;
    [JsonConstructor]
    public Game(int turnNumber, Player active, Player inactive, LargeGrid<Grid<Tile>, Tile> board, TimeSpan turnDelay, TimeSpan transitionTime)
    {
        TurnNumber = turnNumber;
        Active = active;
        Inactive = inactive;
        Board = board;
        TurnDelay = turnDelay;
        TransitionTime = transitionTime;
        InProgress = false;
        var position = new Vector2(450, 350);
        var transform = new Transform2D(position, 0, 4);
        BoardUI = new(board, transform, transitionTime);
        Active.PlayTurn += HandlePlayerTurn;
        Inactive.PlayTurn += HandlePlayerTurn;
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
        Active.EndTurn();
        (Inactive, Active) = (Active, Inactive);
        DelayedPlayTurn();
        TurnNumber++;
    }
    protected void HandlePlayerTurn(Player player, ILargeBoard<Grid<Tile>, Tile> board, (int, int) move)
    {
        if (player != Active)
        {
            Console.WriteLine($"Not player {player}'s turn");
            return;
        }
        Board = Board.Place(Active.GetToken(), move);
        BoardUI.UpdateLargeBoard(Board);
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
        BoardUI.Update();
        if (ChangePlayer && Board.AnyPlaceable)
        {
            ChangePlayer = false;
            NextPlayer();
        }
        Active.Update();
        // Board currently in a transition
        if (BoardUI.InTransition)
        {
            return;
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
        // Toggle players
        BannerController.Activate(Active);
        BannerController.Deactivate(Inactive);
    }
}