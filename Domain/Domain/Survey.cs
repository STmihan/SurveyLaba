using System.Text.Json;

namespace Domain;

public class Survey
{
    public string Name { get; set; } = string.Empty;
    public int Start { get; set; } = 1;
    public List<DynamicDataType> Data { get; set; } = new();
    public List<Question> Questions { get; set; } = new();
    public List<SurveyEnd> Ends { get; set; } = new();

    public List<Answer> Answers { get; set; } = new();

    public Question GetStartQuestion()
    {
        return Questions.FirstOrDefault(q => q.Id == Start) ?? throw new Exception("No start question found");
    }

    public List<Answer> GetAnswers(Question question)
    {
        return Answers.Where(a => a.QuestionId == question.Id).ToList();
    }

    public EndOrQuestion ChooseAnswer(Answer answer)
    {
        if (answer.Action != null)
        {
            var value = DomainUtils.ApplyOperator(answer.Action.Operator, answer.Action.Field.Value, answer.Action.B);
            answer.Action.Field.Value = value;
        }
        
        Question? nextQuestion = Questions.FirstOrDefault(q => q.Id == answer.To);
        if (nextQuestion != null)
        {
            return new EndOrQuestion
            {
                IsEnd = false,
                Question = nextQuestion,
            };
        }
        
        SurveyEnd? end = Ends.FirstOrDefault(e => e.Id == answer.To);
        if (end != null)
        {
            return new EndOrQuestion
            {
                IsEnd = true,
                End = end,
            };
        }
        
        throw new Exception("No next question or end found");
    }
}
