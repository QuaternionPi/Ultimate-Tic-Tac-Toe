using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe;
public static partial class Graphics
{
    public static void ClearBackground(Color color) => Raylib.ClearBackground(color);
    public static void BeginDrawing() => Raylib.BeginDrawing();
    public static void EndDrawing() => Raylib.EndDrawing();
    public static void BeginMode2D(Camera2D camera) => Raylib.BeginMode2D(camera);
    public static void EndMode2D() => Raylib.EndMode2D();
    public static void BeginMode3D(Camera3D camera) => Raylib.BeginMode3D(camera);
    public static void EndMode3D() => Raylib.EndMode3D();
    public static void BeginTextureMode(RenderTexture2D target) => Raylib.BeginTextureMode(target);
    public static void EndTextureMode() => Raylib.EndTextureMode();
    public static void BeginShaderMode(Shader shader) => Raylib.BeginShaderMode(shader);
    public static void EndShaderMode() => Raylib.EndShaderMode();
    public static void BeginBlendMode(BlendMode mode) => Raylib.BeginBlendMode(mode);
    public static void EndBlendMode() => Raylib.EndBlendMode();
    public static void BeginScissorMode(int x, int y, int width, int height) => Raylib.BeginScissorMode(x, y, width, height);
    public static void EndScissorMode() => Raylib.EndScissorMode();

    public static Shader LoadShader(string vsFileName, string fsFileName) => Raylib.LoadShader(vsFileName, fsFileName);
    public static Shader LoadShaderFromMemory(string vsCode, string fsCode) => Raylib.LoadShaderFromMemory(vsCode, fsCode);
    public static int GetShaderLocation(Shader shader, string uniformName) => Raylib.GetShaderLocation(shader, uniformName);
    public static int GetShaderLocationAttrib(Shader shader, string attribName) => Raylib.GetShaderLocationAttrib(shader, attribName);
    public static unsafe void SetShaderValue(Shader shader, int locIndex, void* value, ShaderUniformDataType uniformType) =>
        Raylib.SetShaderValue(shader, locIndex, value, uniformType);
    public static unsafe void SetShaderValueV(Shader shader, int locIndex, void* value, ShaderUniformDataType uniformType, int count) =>
        Raylib.SetShaderValueV(shader, locIndex, value, uniformType, count);
    public static void SetShaderValueMatrix(Shader shader, int locIndex, Matrix4x4 matrix) =>
        Raylib.SetShaderValueMatrix(shader, locIndex, matrix);
    public static void SetShaderValueTexture(Shader shader, int locIndex, Texture2D texture) =>
        Raylib.SetShaderValueTexture(shader, locIndex, texture);
    public static void UnloadShader(Shader shader) => Raylib.UnloadShader(shader);
    public static Ray GetMouseRay(Vector2 mousePosition, Camera3D camera) => Raylib.GetMouseRay(mousePosition, camera);
    public static Matrix4x4 GetCameraMatrix(Camera3D camera) => Raylib.GetCameraMatrix(camera);
    public static Matrix4x4 GetCameraMatrix2D(Camera2D camera) => Raylib.GetCameraMatrix2D(camera);
    public static Vector2 GetWorldToScreen(Vector3 position, Camera3D camera) => Raylib.GetWorldToScreen(position, camera);
    public static Vector2 GetScreenToWorld2D(Vector2 position, Camera2D camera) => Raylib.GetScreenToWorld2D(position, camera);
    public static Vector2 GetWorldToScreenEx(Vector3 position, Camera3D camera, int width, int height) =>
        Raylib.GetWorldToScreenEx(position, camera, width, height);
    public static Vector2 GetWorldToScreen2D(Vector2 position, Camera2D camera) => Raylib.GetWorldToScreen2D(position, camera);
}