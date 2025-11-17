namespace Form.Domain;

public class ContentItem
{
    public required string Id { get; set; }
    public required string ContentType { get; set; }
    public required string Name { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public required object Route { get; set; }
    public Dictionary<string, object?>? Properties { get; set; } = [];
}