using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe
{
    public static partial class Graphics
    {
        public static class Draw
        {
            public static bool OverrideDrawColor;
            public static Color OverrideColor;
            public static void Pixel(int posX, int posY, Color color) => PixelV(new Vector2(posX, posY), color);
            public static void PixelV(Vector2 position, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawPixelV(position, drawColor);
            }
            public static void Line(int startPosX, int startPosY, int endPosX, int endPosY, Color color) =>
                LineV(new Vector2(startPosX, startPosY), new Vector2(endPosX, endPosY), color);
            public static void LineV(Vector2 startPos, Vector2 endPos, Color color) =>
                LineEx(startPos, endPos, 1, color);
            public static void LineEx(Vector2 startPos, Vector2 endPos, float thick, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawLineEx(startPos, endPos, thick, drawColor);
            }
            public static void LineStrip(Vector2[] points, int pointCount, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawLineStrip(points, pointCount, drawColor);
            }
            public static void LineBezier(Vector2 startPos, Vector2 endPos, float thick, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawLineBezier(startPos, endPos, thick, drawColor);
            }
            public static void Circle(int centerX, int centerY, float radius, Color color) =>
                CircleV(new Vector2(centerX, centerY), radius, color);
            public static void CircleSector(Vector2 center, float radius, float startAngle, float endAngle, int segments, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawCircleSector(center, radius, startAngle, endAngle, segments, drawColor);
            }
            public static void CircleSectorLines(Vector2 center, float radius, float startAngle, float endAngle, int segments, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawCircleSectorLines(center, radius, startAngle, endAngle, segments, drawColor);
            }
            public static void CircleGradient(int centerX, int centerY, float radius, Color color1, Color color2)
            {
                Color drawColor1 = OverrideDrawColor ? OverrideColor : color1;
                Color drawColor2 = OverrideDrawColor ? OverrideColor : color2;
                Raylib.DrawCircleGradient(centerX, centerY, radius, drawColor1, drawColor2);
            }
            public static void CircleV(Vector2 center, float radius, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawCircleV(center, radius, drawColor);
            }
            public static void CircleLines(int centerX, int centerY, float radius, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawCircleLines(centerX, centerY, radius, drawColor);
            }
            public static void CircleLinesV(Vector2 center, float radius, Color color) =>
                CircleLines((int)center.X, (int)center.Y, radius, color);
            public static void Ellipse(int centerX, int centerY, float radiusH, float radiusV, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawEllipse(centerX, centerY, radiusH, radiusV, drawColor);
            }
            public static void EllipseLines(int centerX, int centerY, float radiusH, float radiusV, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawEllipseLines(centerX, centerY, radiusH, radiusV, drawColor);
            }
            public static void Ring(Vector2 center, float innerRadius, float outerRadius, float startAngle, float endAngle, int segments, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawRing(center, innerRadius, outerRadius, startAngle, endAngle, segments, drawColor);
            }
            public static void RingLines(Vector2 center, float innerRadius, float outerRadius, float startAngle, float endAngle, int segments, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawRingLines(center, innerRadius, outerRadius, startAngle, endAngle, segments, drawColor);
            }
            public static void Rectangle(int posX, int posY, int width, int height, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawRectangle(posX, posY, width, height, drawColor);
            }
            public static void RectangleV(Vector2 position, Vector2 size, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawRectangleV(position, size, drawColor);
            }
            public static void RectangleRec(Rectangle rec, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawRectangleRec(rec, drawColor);
            }
            public static void RectanglePro(Rectangle rec, Vector2 origin, float rotation, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawRectanglePro(rec, origin, rotation, drawColor);
            }
            public static void RectangleLines(int posX, int posY, int width, int height, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawRectangleLines(posX, posY, width, height, drawColor);
            }
            public static void RectangleLinesEx(Rectangle rec, float lineThick, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawRectangleLinesEx(rec, lineThick, drawColor);
            }
            public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawTriangle(v1, v2, v3, drawColor);
            }
            public static void TriangleLines(Vector2 v1, Vector2 v2, Vector2 v3, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawTriangleLines(v1, v2, v3, drawColor);
            }
            public static void TriangleFan(Vector2[] points, int pointCount, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawTriangleFan(points, pointCount, drawColor);
            }
            public static void TriangleStrip(Vector2[] points, int pointCount, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawTriangleStrip(points, pointCount, drawColor);
            }
            public static void Poly(Vector2 center, int sides, float radius, float rotation, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawPoly(center, sides, radius, rotation, drawColor);
            }
            public static void PolyLines(Vector2 center, int sides, float radius, float rotation, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawPolyLines(center, sides, radius, rotation, drawColor);
            }
            public static void PolyLinesEx(Vector2 center, int sides, float radius, float rotation, float lineThick, Color color)
            {
                Color drawColor = OverrideDrawColor ? OverrideColor : color;
                Raylib.DrawPolyLinesEx(center, sides, radius, rotation, lineThick, drawColor);
            }
        }
    }
}