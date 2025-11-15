namespace Form.Domain;

public class ContentItem
{
    public string ContentType { get; set; }
    public string Name { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public ContentItemRoute Route { get; set; }
    public string Id { get; set; }
    public Dictionary<string, object> Properties { get; set; }
    public Dictionary<string, object> Cultures { get; set; }
}

public class ContentItemRoute
{
    public string Path { get; set; }
    public string QueryString { get; set; }
    public ContentItemStart StartItem { get; set; }
}

public class ContentItemStart
{
    public string Id { get; set; }
    public string Path { get; set; }
}