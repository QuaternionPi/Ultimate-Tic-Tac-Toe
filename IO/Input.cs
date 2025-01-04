using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe;
public static class Keyboard
{
    public static bool IsKeyPressed(KeyboardKey key) => Raylib.IsKeyPressed(key);
    public static bool IsKeyDown(KeyboardKey key) => Raylib.IsKeyDown(key);
    public static bool IsKeyReleased(KeyboardKey key) => Raylib.IsKeyReleased(key);
    public static bool IsKeyUp(KeyboardKey key) => Raylib.IsKeyUp(key);
    public static void SetExitKey(KeyboardKey key) => Raylib.SetExitKey(key);
}
public static class Mouse
{
    public static bool IsMouseButtonPressed(MouseButton button) => Raylib.IsMouseButtonPressed(button);
    public static bool IsMouseButtonDown(MouseButton button) => Raylib.IsMouseButtonDown(button);
    public static bool IsMouseButtonReleased(MouseButton button) => Raylib.IsMouseButtonReleased(button);
    public static bool IsMouseButtonUp(MouseButton button) => Raylib.IsMouseButtonUp(button);
    public static int GetMouseX() => Raylib.GetMouseX();
    public static int GetMouseY() => Raylib.GetMouseY();
    public static Vector2 GetMousePosition() => Raylib.GetMousePosition();
    public static Vector2 GetMouseDelta() => Raylib.GetMouseDelta();
    public static void SetMousePosition(int x, int y) => Raylib.SetMousePosition(x, y);
    public static void SetMouseOffset(int offsetX, int offsetY) => Raylib.SetMouseOffset(offsetX, offsetY);
    public static void SetMouseScale(float scaleX, float scaleY) => Raylib.SetMouseScale(scaleX, scaleY);
    public static float GetMouseWheelMove() => Raylib.GetMouseWheelMove();
    public static Vector2 GetMouseWheelMoveV() => Raylib.GetMouseWheelMoveV();
}