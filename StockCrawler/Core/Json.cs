using System.Text.Json;

namespace StockCrawler;

public static class Json
{
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions() { };
    public static T? LoadJsonData<T>(string dataPath, string filename)
    {
        using StreamReader streamReader = new(Path.Combine(dataPath, filename));
        if (streamReader == null)
            return default;

        string? line = streamReader.ReadLine();
        streamReader.Close();

        if (line == null)
            return default;

        T? obj = JsonSerializer.Deserialize<T?>(line);
        return obj;
    }
    public static void SaveDatasAsJson<T>(T obj, string dataPath, string filename)
    {
        if (obj == null || dataPath.Length == 0 || filename.Length == 0)
            return;

        if (!Directory.Exists(dataPath))
            _ = Directory.CreateDirectory(dataPath);

        using StreamWriter streamWriter = new(Path.Combine(dataPath, filename));
        streamWriter.WriteLine(JsonSerializer.Serialize(obj, options));
        streamWriter.Flush();
        streamWriter.Close();
    }
}