using System.Net.Http.Json;
using System.Text.Json;
using Form.Core;
using Form.Domain;

public class UmbracoService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("UmbracoClient");

    public async Task<CmsNode> GetContentAndChildren(string id)
    {
        var content = await GetContent(id);
        var children = await GetChildren(id);
        content.Children = children;

        return content;
    }

    public async Task<CmsNode> GetContent(string id)
    {
        var (content, ex) = await TryHelper.TryAsync(() => _httpClient.GetFromJsonAsync<ContentItem>($"content/item/{id}"));

        if (content == null)
            throw ex!;

        return CmsNode.CreateNode(content.ContentType, content.Properties);
    }

    public async Task<List<CmsNode>> GetChildren(string id)
    {
        var (json, ex) = await TryHelper.TryAsync(() => _httpClient.GetFromJsonAsync<JsonDocument>($"content?fetch=descendants:{id}"));

        if (json == null)
            throw ex!;

        var items = json.RootElement.GetProperty("items");

        if (items.ValueKind != JsonValueKind.Array) throw new ArgumentException("Expected items array!");

        var nodeMap = new Dictionary<string, CmsNode>();
        var children = new List<CmsNode>();

        foreach (JsonElement element in items.EnumerateArray())
        {
            var path = element.GetProperty("route").GetProperty("path").GetString()?.Trim('/');
            var node = CmsNode.CreateFromJson(element);

            if (path == null)
                throw new ArgumentException("Missing path value");

            var index = path.LastIndexOf("/");
            if (index == -1)
            {
                children.Add(node);
                continue;
            }

            var parentKey = path[..index];

            nodeMap[path] = node;
            if (nodeMap.TryGetValue(parentKey, out var parent))
            {
                parent.Children.Add(node);
                continue;
            }

            children.Add(node);
        }

        return children;
    }
}
