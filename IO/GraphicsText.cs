using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe;
public static partial class Graphics
{
    public static class Text
    {
        public static Font GetFontDefault() => Raylib.GetFontDefault();
        public static void DrawFPS(int posX, int posY) => Raylib.DrawFPS(posX, posY);
        public static void DrawText(string text, int posX, int posY, int fontSize, Color color) =>
            Raylib.DrawText(text, posX, posY, fontSize, color);
        public static void DrawTextEx(Font font, string text, Vector2 position, float fontSize, float spacing, Color tint) =>
            Raylib.DrawTextEx(font, text, position, fontSize, spacing, tint);
        public static void DrawTextPro(Font font, string text, Vector2 position, Vector2 origin, float rotation, float fontSize, float spacing, Color tint) =>
            Raylib.DrawTextPro(font, text, position, origin, rotation, fontSize, spacing, tint);
        public static Vector2 MeasureTextEx(Font font, string text, float fontSize, float spacing) =>
            Raylib.MeasureTextEx(font, text, fontSize, spacing);
    }
}