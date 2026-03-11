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
        Directory.CreateDirectory(dir);            // ensure folder always exists
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

        // BUG FIX: initialise current from root before walking segments
        JsonElement current = root;

        foreach (var segment in token.Split('.'))
        {
            current = current.GetProperty(segment);
        }

        return current.Deserialize<T>()!;
    }

    // ─── READ: load the entire file and deserialize to T (for flat objects) ───
    /// <summary>
    /// Deserializes an entire JSON file to T.
    /// Useful for reading saved credentials: GetCredentials&lt;UserCredentials&gt;("credentials.json")
    /// </summary>
    public static T GetCredentials<T>(string fileName)
    {
        var path = ResolvePath(fileName);

        var root = _cache.GetOrAdd(path, p =>
        {
            var json = File.ReadAllText(p);
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.Clone();
        });

        return root.Deserialize<T>()!;
    }

    // ─── WRITE: serialize any object to a JSON file ───────────────────────────
    /// <summary>
    /// Serializes <paramref name="data"/> and writes it to TestData/<paramref name="fileName"/>.
    /// Also invalidates the cache so the next read picks up the new content.
    /// </summary>
    public static void SaveCredentials<T>(string fileName, T data)
    {
        var path = ResolvePath(fileName);

        var json = JsonSerializer.Serialize(data, _writeOptions);
        File.WriteAllText(path, json);

        // Invalidate cache so subsequent reads reflect the new file
        _cache.TryRemove(path, out _);
    }
}