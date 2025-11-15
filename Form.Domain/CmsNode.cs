using System.Text.Json;

namespace Form.Domain;

public interface ICmsContent
{
    public string ContentType { get; set; }
}

public class CmsNode<T> where T : ICmsContent
{
    private CmsNode() { }
    public T Content { get; set; } = default!;
    public List<CmsNode<T>> Children { get; set; } = [];

    public static CmsNode<T> CreateNode(T content, List<CmsNode<T>>? children = null)
    {
        return new CmsNode<T>
        {
            Content = content,
            Children = children ?? []
        };
    }
}

public class CmsNode
{
    private CmsNode() { }
    public Dictionary<string, object> Content { get; set; } = [];
    public List<CmsNode> Children { get; set; } = [];
    public static CmsNode CreateNode(string contentType, Dictionary<string, object>? properties = null, List<CmsNode>? children = null)
    {
        var node = new CmsNode();

        node.Content["contentType"] = contentType;

        if (properties != null)
            foreach (var prop in properties)
            {
                node.Content[prop.Key] = prop.Value;
            }

        node.Children = children ?? [];

        return node;
    }

    public static CmsNode CreateFromJson(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Object)
            throw new ArgumentException("Expected object inside items!");

        var contentType = element.GetProperty("contentType").Deserialize<string>();
        var jsonProperties = element.GetProperty("properties");
        var properties = new Dictionary<string, object>();

        foreach (var prop in jsonProperties.EnumerateObject())
        {
            var text = prop.Value.GetRawText();
            if (BlockListProperty.IsBlockList(text))
            {
                properties[prop.Name] = BlockListProperty.CreateFromJson(prop.Value);
            }
            else
            {
                properties[prop.Name] = prop.Value.ValueKind == JsonValueKind.Null ? null : prop.Value.Deserialize<object>();
            }
        }


        if (contentType == null)
            throw new ArgumentException("Missing contentType or path");

        var node = CreateNode(contentType, properties);

        return node;
    }
}


