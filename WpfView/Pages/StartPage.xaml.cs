using System;
using System.Windows;
using Domain;

namespace WpfView.pages;

public partial class StartPage
{
    private readonly Survey _survey;
    
    public StartPage(Survey survey)
    {
        _survey = survey;
        InitializeComponent();
        SurveyName.Text = _survey.Name;
    }
    
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Console.WriteLine(_survey);
    }
}