using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Nodes;

public static class JsonHelper
{
    private static readonly ConcurrentDictionary<string, JsonElement> _cache = new();

    private static readonly JsonSerializerOptions _writeOptions = new()
    {
        WriteIndented = true
    };

    // ─── Private helper: resolve the TestData folder path ────────────────────
    private static string ResolvePath(string fileName)
    {
        var dir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "TestData"));
        Directory.CreateDirectory(dir);
        return Path.Combine(dir, fileName);
    }

    // ─── READ: navigate a dot-separated token path and deserialize ────────────
    /// <summary>
    /// Reads a value from a JSON test-data file using a dot-separated token path.
    /// Example: GetTestData&lt;string&gt;("config.json", "user.email")
    /// </summary>
    public static T GetTestData<T>(string fileName, string token)
    {
        var path = ResolvePath(fileName);

        var root = _cache.GetOrAdd(path, p =>
        {
            var json = File.ReadAllText(p);
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.Clone();
        });

        JsonElement current = root;

        foreach (var segment in token.Split('.'))
        {
            current = current.GetProperty(segment);
        }

        return current.Deserialize<T>()!;
    }

    // ─── WRITE: append an entry to a JSON array section ──────────────────────
    /// <summary>
    /// Appends <paramref name="data"/> to a JSON array at <paramref name="sectionKey"/>.
    /// Creates the array if it does not exist yet.
    /// Example: AppendToSection("Credentials.json", "NewUserCreated", credentials)
    /// </summary>
    public static void AppendToSection<T>(string fileName, string sectionKey, T data)
    {
        var path = ResolvePath(fileName);

        // Load existing file or start with empty object
        var rootNode = File.Exists(path)
            ? JsonNode.Parse(File.ReadAllText(path))!.AsObject()
            : new JsonObject();

        // Get existing array or create a new one
        var array = rootNode[sectionKey]?.AsArray() ?? new JsonArray();

        // Append the new entry
        array.Add(JsonNode.Parse(JsonSerializer.Serialize(data, _writeOptions)));

        rootNode[sectionKey] = array;

        File.WriteAllText(path, rootNode.ToJsonString(_writeOptions));

        _cache.TryRemove(path, out _);
    }
}