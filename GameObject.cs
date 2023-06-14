using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

static class GameManager
{
    public static void UpdateAll()
    {
        List<UpdateFunction> updates = new List<UpdateFunction>(UpdateFunctions);
        foreach (UpdateFunction function in updates)
        {
            function();
        }
    }
    public static void DrawAll()
    {
        foreach (DrawFunction function in DrawFunctions)
        {
            function();
        }
    }
    public static void AddUpdate(UpdateFunction function)
    {
        if (UpdateFunctions.Contains(function))
        {
            return;
        }
        UpdateFunctions.Add(function);
    }
    public static void RemoveUpdate(UpdateFunction function)
    {
        if (UpdateFunctions.Contains(function))
        {
            UpdateFunctions.Remove(function);
        }
    }
    public static void AddDraw(DrawFunction function)
    {
        if (DrawFunctions.Contains(function))
        {
            return;
        }
        DrawFunctions.Add(function);
    }
    public static void RemoveDraw(DrawFunction function)
    {
        if (DrawFunctions.Contains(function))
        {
            DrawFunctions.Remove(function);
        }
    }
    public delegate void UpdateFunction();
    public delegate void DrawFunction();
    private static List<UpdateFunction> UpdateFunctions = new List<UpdateFunction>();
    private static List<DrawFunction> DrawFunctions = new List<DrawFunction>();
}

class GameObject
{
    public GameObject()
    {
        GameManager.AddUpdate(this.Update);
        GameManager.AddDraw(this.Draw);
    }
    ~GameObject()
    {

    }
    public virtual void OnLeftClick()
    {

    }
    public virtual void OnRightClick()
    {

    }
    public virtual void Update()
    {

    }
    public virtual void Draw()
    {

    }
    public virtual void OnDestroy()
    {

    }
    public void Destroy()
    {
        OnDestroy();
        GameManager.RemoveUpdate(this.Update);
        GameManager.RemoveDraw(this.Draw);
    }
}