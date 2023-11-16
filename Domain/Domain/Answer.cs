namespace Domain;

public class SurveyAction
{
    public required DynamicDataField Field;
    public required string Operator;
    public required object B;
}

public class Answer
{
    public int QuestionId { get; set; }
    public int To { get; set; }
    public string Text { get; set; } = string.Empty;
    public SurveyAction? Action { get; set; } = null;
}
