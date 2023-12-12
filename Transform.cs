using System.Numerics;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe
{
    /*
    Position, Rotation and Scale in 2D space
    */
    public readonly struct Transform2D
    {
        public Transform2D(Vector2 position, float rotation = 0, float scale = 1)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
        [JsonInclude]
        [JsonConverter(typeof(Json.Vector2Converter))]
        public Vector2 Position
        {
            get;
        }
        [JsonInclude]
        public float Rotation
        {
            get;
        }
        [JsonInclude]
        public float Scale
        {
            get;
        }
        public Transform2D Translate(Vector2 delta)
        {
            return new Transform2D(Position + delta, Rotation, Scale);
        }
    }
}