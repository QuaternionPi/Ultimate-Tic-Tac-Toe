using System.Text.Json.Serialization;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToe.Serialization;

public struct Save
{
    public bool IsAutoSave { get; set; }
    public string Name { get; set; }
    public DateTime DateTime { get; set; }
    public Game.Game Game { get; }
    [JsonIgnore]
    internal readonly string File => $"{Name}.save.json";
    [JsonConstructor]
    internal Save(bool isAutoSave, string name, DateTime dateTime, Game.Game game)
    {
        IsAutoSave = isAutoSave;
        Name = name;
        DateTime = dateTime;
        Game = game;
    }
}