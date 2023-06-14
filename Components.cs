using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

interface ITransform
{
    Vector2 Position
    {
        get;
    }
    float Rotation
    {
        get;
    }
    float Scale
    {
        get;
    }
}