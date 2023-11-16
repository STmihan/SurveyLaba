namespace Domain;

public class SurveyEnd
{
    public int Id { get; set; }
    public string Text => Value.ToString() ?? "No end text";
    public object Value { get; set; } = string.Empty;
}
