using System.Text.Json;
using UltimateTicTacToe.Serialization.Json;

namespace UltimateTicTacToe.Serialization;

public static class SaveManager
{
    private static readonly string SaveLocation = "../../../../saves";
    public static List<Save> AllSaves(string? saveLocation = null)
    {
        string target = saveLocation ?? SaveLocation;
        var options = GetOptions();

        Directory.CreateDirectory(target);
        var files = Directory.GetFiles(target, "*.save.json");
        List<Save> saves = [..
            from file in files
            let json = File.ReadAllText(file)
            let save = JsonSerializer.Deserialize<Save>(json, options)
            orderby save.DateTime descending
            select save
        ];
        return saves;
    }
    public static bool Write(string name, Game.Game game, bool isAutoSave = false, string? saveLocation = null)
    {
        var dateTime = DateTime.Now;
        var save = new Save(isAutoSave, name, dateTime, game);
        var options = GetOptions();
        try
        {
            var json = JsonSerializer.Serialize(save, options);
            string target = saveLocation ?? SaveLocation;

            Directory.CreateDirectory(target);
            var file = Path.Join(target, save.File);
            File.WriteAllText(file, json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        return true;
    }
    private static JsonSerializerOptions GetOptions()
    {
        return new JsonSerializerOptions()
        {
            Converters = {
                new ColorConverter(),
                new GridOfTConverter(),
                new LargeGridOfTConverter()
            }
        };
    }
}