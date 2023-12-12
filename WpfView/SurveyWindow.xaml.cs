using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Domain;

namespace WpfView;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class SurveyWindow
{
    private readonly Survey _survey;
    private EndOrQuestion _endOrQuestion;
    private Question _nextQuestion;

    public SurveyWindow(Survey survey)
    {
        _survey = survey;
        InitializeComponent();
        _nextQuestion = _survey.GetStartQuestion();
        SetupQuestion(_nextQuestion);
    }

    private void CreateAnswersButton(Question question)
    {
        AnswersStackPanel.Children.Clear();
        List<Answer> answers = _survey.GetAnswers(question);
        for (int i = 0; i < answers.Count; i++)
        {
            Answer answer = answers[i];
            Button button = new()
            {
                Content = $"{i + 1} {answer.Text}",
                Tag = answer,
            };
            button.Click += (_, _) => ButtonOnClick(answer);
            AnswersStackPanel.Children.Add(button);
        }
    }

    private void ButtonOnClick(Answer answer)
    {
        _endOrQuestion = _survey.ChooseAnswer(answer);
        if (_endOrQuestion.IsEnd)
        {
            EndSurvey();
            return;
        }

        _nextQuestion = _endOrQuestion.Question ?? throw new Exception("No next question found");
        SetupQuestion(_nextQuestion);
    }

    private void EndSurvey()
    {
        string endText = _endOrQuestion.End?.Text ?? throw new Exception("No end found");
        MessageBox.Show(endText);
        var startSurveyWindow = new StartSurveyWindow(_survey);
        startSurveyWindow.Show();
        Close();
    }
    
    private void SetupQuestion(Question nextQuestion)
    {
        QuestionTextBlock.Text = nextQuestion.Text;
        
        CreateAnswersButton(nextQuestion);
    }
}