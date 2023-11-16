// See https://aka.ms/new-console-template for more information

using System.Text;
using Domain;

namespace CLI;

public static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a path to a srv file");
            return;
        }
        Survey survey = Parser.SRVToSurvey(File.ReadAllText(args[0], Encoding.UTF8));
        EndOrQuestion endOrQuestion = new();
        Question nextQuestion = survey.GetStartQuestion();
        while (!endOrQuestion.IsEnd)
        {
            Console.WriteLine(nextQuestion.Text);
            List<Answer> answers = survey.GetAnswers(nextQuestion);
            
            for (int i = 0; i < answers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {answers[i].Text}");
            }

            Console.WriteLine("----------------------------------------");
            Console.Write("Введите ваш ответ: ");
            int answerIndex = int.Parse(Console.ReadLine() ?? string.Empty);
            Answer answer = answers[answerIndex - 1];
            endOrQuestion = survey.ChooseAnswer(answer);

            if (!endOrQuestion.IsEnd)
            {
                nextQuestion = endOrQuestion.Question ?? throw new Exception("No next question found");
            }
        }

        Console.WriteLine(endOrQuestion.End?.Text ?? throw new Exception("No end found"));
    }
}
