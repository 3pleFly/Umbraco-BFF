using System.Text.Json;
using System.Text.RegularExpressions;

namespace Form.Domain;

public class BlockListProperty
{
    public static bool IsBlockList(string json)
    {
        var regex = new Regex("{\\\"items\\\":\\[", RegexOptions.Compiled);

        return regex.IsMatch(json);
    }
    public static IEnumerable<Dictionary<string, object>> CreateFromJson(JsonElement json)
    {
        var items = json.GetProperty("items");

        var list = new List<Dictionary<string, object>>();

        foreach (var item in items.EnumerateArray())
        {
            var content = item.GetProperty("content");
            var contentType = content.GetProperty("contentType").GetString()!;
            var node = new Dictionary<string, object>() { { "contentType", contentType } };

            foreach (var property in content.GetProperty("properties").EnumerateObject())
            {
                var value = property.Value.ValueKind == JsonValueKind.Null ? null : property.Value.Deserialize<object>();
                node.Add(property.Name, value!);
            }

            list.Add(node);
        }

        return list;
    }
}