using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Window.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
            Window.Init(900, 650, "Ultimate Tic Tac Toe");
            Window.SetTargetFPS(30);

            Image windowIcon = Raylib_cs.Raylib.LoadImage("./Window Icon.png");
            Window.SetWindowIcon(windowIcon);
            Raylib_cs.Raylib.UnloadImage(windowIcon);

            Mode.SwitchTo += ChangeMode;
            while (!Window.ShouldClose())
            {
                Mode.Update();
                Graphics.BeginDrawing();
                Graphics.ClearBackground(Color.RAYWHITE);

                Mode.Draw();
                Graphics.EndDrawing();
            }
        }
        private static IProgramMode Mode = new Setup();
        private static void ChangeMode(IProgramMode from, IProgramMode to)
        {
            from.SwitchTo -= ChangeMode;
            Mode = to;
            to.SwitchTo += ChangeMode;
        }
    }
    public interface IProgramMode : IDrawable, IUpdateable, ITransitionable
    {
        public event SwitchToDel? SwitchTo;
        public delegate void SwitchToDel(IProgramMode sender, IProgramMode switchTo);
        public IProgramMode? Previous { get; }
    }
    public class Setup : IProgramMode
    {
        public Setup()
        {
            Player1 = new Human(Player.Symbol.X, Color.RED, 0);
            Player2 = new Human(Player.Symbol.O, Color.BLUE, 0);
            UI = new UI.BannerControler(new Player[] { Player1, Player2 });
            UI.Activate(Player1);
            UI.Activate(Player2);

            var colors = Player.AllowedColors;
            RightColorPicker = new UI.ColorPicker(new Transform2D(new Vector2(900 - 135, 270)), colors, 3);
            LeftColorPicker = new UI.ColorPicker(new Transform2D(new Vector2(35, 270)), colors, 3);

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

            var playButton = new UI.Button(playButtonTransform, new Vector2(400, 70), "Play", textColor, backgroundColor, borderColor);
            var rightHumanButton = new UI.Button(rightHumanButtonTransform, new Vector2(150, 70), "Human", textColor, backgroundColor, borderColor);
            var leftHumanButton = new UI.Button(leftHumanButtonTransform, new Vector2(150, 70), "Human", textColor, backgroundColor, borderColor);
            var rightBotButton = new UI.Button(rightBotButtonTransform, new Vector2(150, 70), "Bot", textColor, backgroundColor, borderColor);
            var leftBotButton = new UI.Button(leftBotButtonTransform, new Vector2(150, 70), "Bot", textColor, backgroundColor, borderColor);

            playButton.Clicked += SetupGame;
            rightHumanButton.Clicked += SetPlayer1Human;
            leftHumanButton.Clicked += SetPlayer2Human;
            rightBotButton.Clicked += SetPlayer1Bot;
            leftBotButton.Clicked += SetPlayer2Bot;

            Buttons = new List<UI.Button>(){
                playButton,
                rightHumanButton,
                leftHumanButton,
                rightBotButton,
                leftBotButton,
            };
        }
        protected List<UI.Button> Buttons;
        public bool InTransition { get; }
        public float TransitionValue { get; }
        public event IProgramMode.SwitchToDel? SwitchTo;
        public IProgramMode? Previous { get; }
        protected UI.BannerControler UI { get; }
        protected UI.ColorPicker RightColorPicker { get; }
        protected UI.ColorPicker LeftColorPicker { get; }
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
            Buttons.ForEach(button => button.Update());
            RightColorPicker.Update();
            LeftColorPicker.Update();
        }
        protected void SetupGame()
        {
            Player player1;
            Player player2;
            if (Player1Type == "Human")
                player1 = new Human(Player1.Shape, Player1.Color, 0);
            else
                player1 = new Bot(Player1.Shape, Player1.Color, 0);
            if (Player2Type == "Human")
                player2 = new Human(Player2.Shape, Player2.Color, 0);
            else
                player2 = new Bot(Player2.Shape, Player2.Color, 0);
            IProgramMode mode = new PlayGame(player1, player2);
            SwitchTo?.Invoke(this, mode);
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
    public class Save
    {

    }
    public class Load
    {

    }
    public class PlayGame : IProgramMode
    {
        public PlayGame(Player player1, Player player2)
        {

            Vector2 position = new Vector2(450, 350);
            Game = new Game.Game(player1, player2, position);
        }
        public bool InTransition { get; }
        public float TransitionValue { get; }
        public event IProgramMode.SwitchToDel? SwitchTo;
        public IProgramMode? Previous { get; }
        protected Game.Game Game;
        public void Draw()
        {
            Game.Draw();
        }
        public void Update()
        {
            Game.Update();
        }
    }
}