using System;
using System.IO;
using System.Windows;
using Domain;
using Microsoft.Win32;

namespace WpfView;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        string fileName = GetSurveyFileName();
        Console.WriteLine(fileName);
        string content = File.ReadAllText(fileName);
        Survey survey = Parser.SRVToSurvey(content);

        var window = new StartSurveyWindow(survey);
        window.Show();
    }

    private string GetSurveyFileName()
    {
        FileDialog dialog = new OpenFileDialog();
        dialog.InitialDirectory = Directory.GetCurrentDirectory();
        dialog.Filter = "SRV files (*.srv)|*.srv";
        dialog.ShowDialog();
        return dialog.FileName;
    }
}