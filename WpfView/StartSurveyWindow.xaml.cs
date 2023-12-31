﻿using System.Windows;
using Domain;

namespace WpfView;

public partial class StartSurveyWindow
{
    private readonly Survey _survey;

    public StartSurveyWindow(Survey survey)
    {
        _survey = survey;
        InitializeComponent();
        SurveyName.Text = _survey.Name;
        
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        SurveyWindow surveyWindow = new(_survey);
        surveyWindow.Show();
        Close();
    }
}