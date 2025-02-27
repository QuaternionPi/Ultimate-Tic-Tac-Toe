using System.Numerics;
using Raylib_cs;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToe.UI.ProgramMode;

public class Setup : IProgramMode
{
    public Setup(IProgramMode previous)
    {
        Previous = previous;
        Player1 = new Human(Player.Symbol.X, Color.RED, 0);
        Player2 = new Human(Player.Symbol.O, Color.BLUE, 0);
        UI = new BannerController(Player1, Player2);
        UI.Activate(Player1);
        UI.Activate(Player2);

        var colors = Player.AllowedColors;
        RightColorPicker = new ColorPicker(new Transform2D(new Vector2(900 - 135, 270)), colors, 3);
        LeftColorPicker = new ColorPicker(new Transform2D(new Vector2(35, 270)), colors, 3);

        RightColorPicker.Clicked += SetPlayer1Color;
        LeftColorPicker.Clicked += SetPlayer2Color;

        Player1Type = "Human";
        Player2Type = "Bot";

        var playButtonTransform = new Transform2D(new Vector2(450, 300));
        var rightHumanButtonTransform = new Transform2D(new Vector2(900 - 85, 400));
        var leftHumanButtonTransform = new Transform2D(new Vector2(85, 400));
        var rightBotButtonTransform = new Transform2D(new Vector2(900 - 85, 500));
        var leftBotButtonTransform = new Transform2D(new Vector2(85, 500));

        var textColor = Color.GRAY;
        var backgroundColor = Color.LIGHTGRAY;
        var borderColor = backgroundColor;

        var playButton = new Button(playButtonTransform, new Vector2(400, 70), "Play", textColor, backgroundColor, borderColor);
        var rightHumanButton = new Button(rightHumanButtonTransform, new Vector2(150, 70), "Human", textColor, backgroundColor, borderColor);
        var leftHumanButton = new Button(leftHumanButtonTransform, new Vector2(150, 70), "Human", textColor, backgroundColor, borderColor);
        var rightBotButton = new Button(rightBotButtonTransform, new Vector2(150, 70), "Bot", textColor, backgroundColor, borderColor);
        var leftBotButton = new Button(leftBotButtonTransform, new Vector2(150, 70), "Bot", textColor, backgroundColor, borderColor);

        playButton.Clicked += SetupGame;
        rightHumanButton.Clicked += SetPlayer1Human;
        leftHumanButton.Clicked += SetPlayer2Human;
        rightBotButton.Clicked += SetPlayer1Bot;
        leftBotButton.Clicked += SetPlayer2Bot;

        Buttons = [playButton, rightHumanButton, leftHumanButton, rightBotButton, leftBotButton];

        SetPlayer1Bot();
        SetPlayer2Bot();
    }
    protected List<Button> Buttons;
    public bool InTransition { get; }
    public float TransitionValue { get; }
    public event Action<IProgramMode, IProgramMode>? SwitchTo;
    public IProgramMode Previous { get; }
    protected BannerController UI { get; }
    protected ColorPicker RightColorPicker { get; }
    protected ColorPicker LeftColorPicker { get; }
    protected Player Player1;
    protected Player Player2;
    protected string Player1Type;
    protected string Player2Type;
    public void Draw()
    {
        Buttons.ForEach(button => button.Draw());
        RightColorPicker.Draw();
        LeftColorPicker.Draw();
        UI.Draw();
    }
    public void Update()
    {
        if (Keyboard.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
        {
            SwitchTo?.Invoke(this, Previous);
        }
        Buttons.ForEach(button => button.Update());
        RightColorPicker.Update();
        LeftColorPicker.Update();
    }
    protected void SetupGame()
    {
        var centre = new BoardEvaluator(80, 20, 40, 100);
        var edge = new BoardEvaluator(20, 5, 10, 25);
        var corner = new BoardEvaluator(40, 10, 20, 50);
        var evaluator = new LargeBoardEvaluator(centre, edge, corner, 1000);
        Player player1;
        Player player2;
        if (Player1Type == "Human")
            player1 = new Human(Player1.Shape, Player1.Color, 0);
        else
            player1 = new Bot(evaluator, new Random(0), Player1.Shape, Player1.Color, 0);

        if (Player2Type == "Human")
            player2 = new Human(Player2.Shape, Player2.Color, 0);
        else
            player2 = new Bot(evaluator, new Random(1), Player2.Shape, Player2.Color, 0);


        var position = new Vector2(450, 350);
        var transform = new Transform2D(position, 0, 4);
        var board = EmptyLargeGrid();

        var turnDelay = new TimeSpan(0, 0, 0, 0, 00);
        var transitionTime = new TimeSpan(0, 0, 0, 0, 00);
        var game = new Game.Game(0, player1, player2, board, board, turnDelay, transitionTime);
        IProgramMode mode = new PlayGame(this, game);
        SwitchTo?.Invoke(this, mode);
    }
    protected static LargeGrid<Grid<Tile>, Tile> EmptyLargeGrid()
    {
        var cells = new Grid<Tile>[9];
        for (int i = 0; i < 9; i++)
        {
            cells[i] = EmptyGrid();
        }
        var victoryTile = new Tile(null);
        return new LargeGrid<Grid<Tile>, Tile>(cells, victoryTile);
    }
    protected static Grid<Tile> EmptyGrid()
    {
        var cells = new Tile[9];
        for (int i = 0; i < 9; i++)
        {
            cells[i] = new Tile(null);
        }
        var victoryTile = new Tile(null);
        return new Grid<Tile>(cells, victoryTile);
    }
    protected void SetPlayer1Color(Color color) => Player1.Color = color;
    protected void SetPlayer2Color(Color color) => Player2.Color = color;
    protected void SetPlayer1Human()
    {
        Player1Type = "Human";
        Buttons[1].BackgroundColor = Color.RAYWHITE;
        Buttons[3].BackgroundColor = Color.LIGHTGRAY;
    }
    protected void SetPlayer2Human()
    {
        Player2Type = "Human";
        Buttons[2].BackgroundColor = Color.RAYWHITE;
        Buttons[4].BackgroundColor = Color.LIGHTGRAY;
    }
    protected void SetPlayer1Bot()
    {
        Player1Type = "Bot";
        Buttons[3].BackgroundColor = Color.RAYWHITE;
        Buttons[1].BackgroundColor = Color.LIGHTGRAY;
    }
    protected void SetPlayer2Bot()
    {
        Player2Type = "Bot";
        Buttons[4].BackgroundColor = Color.RAYWHITE;
        Buttons[2].BackgroundColor = Color.LIGHTGRAY;
    }
}