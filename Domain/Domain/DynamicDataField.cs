namespace Domain;

public class DynamicDataField
{
    public string FieldId { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public required dynamic Value { get; set; }
    public Dictionary<dynamic, string> Mapping { get; set; } = new();
}