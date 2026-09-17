using System.IO;
using System.Text.Json;

namespace ChatGPTMultiView;

public static class TitleStore
{
    public static string[] Load(string path)
    {
        if (!File.Exists(path))
        {
            return DefaultTitles();
        }

        var titles = JsonSerializer.Deserialize<string[]>(File.ReadAllText(path));
        return titles is { Length: 4 } ? titles : DefaultTitles();
    }

    public static void Save(string path, IReadOnlyList<string> titles)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(path, JsonSerializer.Serialize(titles));
    }

    private static string[] DefaultTitles() =>
        ["ChatGPT 1", "ChatGPT 2", "ChatGPT 3", "ChatGPT 4"];
}
