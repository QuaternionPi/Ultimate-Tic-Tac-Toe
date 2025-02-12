using Raylib_cs;
using UltimateTicTacToe.UI.ProgramMode;

namespace UltimateTicTacToe;
public static class Program
{
    static void Main(string[] args)
    {
        Window.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
        Window.Init(900, 650, "Ultimate Tic Tac Toe");
        Window.SetTargetFPS(30);

        Image windowIcon = Raylib.LoadImage("../../../Window Icon.png");
        Window.SetWindowIcon(windowIcon);
        Raylib.UnloadImage(windowIcon);

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
    private static IProgramMode Mode = new Home();
    private static void ChangeMode(IProgramMode from, IProgramMode to)
    {
        from.SwitchTo -= ChangeMode;
        Mode = to;
        to.SwitchTo += ChangeMode;
    }
    public static void Exit(int status = 0)
    {
        Environment.Exit(status);
    }
}
public class Save
{

}
public class Load
{

}