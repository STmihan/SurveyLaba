namespace Domain;

public struct EndOrQuestion
{
    public bool IsEnd { get; set; }
    public SurveyEnd? End { get; set; }
    public Question? Question { get; set; }
}
