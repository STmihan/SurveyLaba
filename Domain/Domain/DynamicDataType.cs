namespace Domain;

public class DynamicDataType
{
    public string Name { get; set; } = string.Empty;
    public List<DynamicDataField> Fields { get; set; } = new();

    public override string ToString()
    {
        string result = string.Empty;
        foreach (DynamicDataField field in Fields)
        {
            string valueStr = field.Mapping.TryGetValue(field.Value, out string mappedValue) ? mappedValue : field.Value.ToString();
            result += $"{field.FieldName}: {valueStr}\n";
        }
        return result;
    }
}