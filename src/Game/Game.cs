using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;
public class Game : IDrawable, IUpdatable
{
    public Game(Player active, Player inactive, LargeGrid<Grid<Tile>, Tile> board)
    {
        _board = board;

        ActivePlayer = active;
        InactivePlayer = inactive;
        ActivePlayer.PlayTurn += HandlePlayerTurn;
        InactivePlayer.PlayTurn += HandlePlayerTurn;
        BannerController = new UI.BannerController(active, inactive);


        TimeSpan delay = new(0, 0, 0, 0, 100);
        Thread thread = new(() => DelayedPlayerStart(delay));
        thread.Start();
    }
    [JsonInclude]
    public LargeGrid<Grid<Tile>, Tile> Board
    {
        get
        {
            return _board;
        }
        protected set
        {
            _board = value;
        }
    }
    private LargeGrid<Grid<Tile>, Tile> _board;
    [JsonInclude]
    public Player ActivePlayer { get; protected set; }
    [JsonInclude]
    public Player InactivePlayer { get; protected set; }
    protected bool ChangePlayer;
    [JsonInclude]
    public UI.BannerController BannerController { get; protected set; }
    [JsonInclude]
    public int TurnNumber { get; protected set; }
    public delegate void GameOverDel(Game sender, Player? winner);
    public event GameOverDel? GameOver;
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
    protected void HandlePlayerTurn(Player player, IEnumerable<ICell> cells)
    {
        if (player != ActivePlayer)
        {
            Console.WriteLine($"Not player {player}'s turn");
            return;
        }
        Board = (LargeGrid<Grid<Tile>, Tile>)Board.Place(cells.Skip(1), ActivePlayer, true);
        ChangePlayer = true;
    }
    protected void DelayedPlayerStart(TimeSpan delay)
    {
        Thread.Sleep(delay);
        ActivePlayer.BeginTurn(Board, InactivePlayer);
    }
    public void Draw()
    {
        Board.Draw();
        BannerController.Draw();
    }
    public void Update()
    {
        Board.Update();
        if (ChangePlayer && Board.Placeable)
        {
            NextPlayer();
            ChangePlayer = false;
        }
        ActivePlayer.Update();
        // Board currently in a transition
        if (Board.TransitionValue != 0)
        {
            return;
        }
        // Board won by a player or the board cannot be placed on
        if (Board.Player != null || Board.Placeable == false)
        {
            GameOver?.Invoke(this, Board.Player);
            return;
        }
        // Toggle players
        BannerController.Activate(ActivePlayer);
        BannerController.Deactivate(InactivePlayer);
    }
}