using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe;
/*
Determines if two objects are colliding
*/
public static class CheckCollision
{
    public static bool Recs(Rectangle rec1, Rectangle rec2) =>
        Raylib.CheckCollisionRecs(rec1, rec2);
    public static bool Circles(Vector2 center1, float radius1, Vector2 center2, float radius2) =>
        Raylib.CheckCollisionCircles(center1, radius1, center2, radius2);
    public static bool CircleRec(Vector2 center, float radius, Rectangle rec) =>
        Raylib.CheckCollisionCircleRec(center, radius, rec);
    public static bool PointRec(Vector2 point, Rectangle rec) =>
        Raylib.CheckCollisionPointRec(point, rec);
    public static bool PointCircle(Vector2 point, Vector2 center, float radius) =>
        Raylib.CheckCollisionPointCircle(point, center, radius);
    public static bool PointTriangle(Vector2 point, Vector2 p1, Vector2 p2, Vector2 p3) =>
        Raylib.CheckCollisionPointTriangle(point, p1, p2, p3);
    public static bool Lines(Vector2 startPos1, Vector2 endPos1, Vector2 startPos2, Vector2 endPos2, Vector2 collisionPoint) =>
        Raylib.CheckCollisionLines(startPos1, endPos1, startPos2, endPos2, ref collisionPoint);
    public static bool PointLine(Vector2 point, Vector2 p1, Vector2 p2, int threshold) =>
        Raylib.CheckCollisionPointLine(point, p1, p2, threshold);
}