using System.Windows.Controls;
using Domain;

namespace WpfView.Pages;

public partial class MainWindow
{
    
    private readonly Survey _survey;
    private EndOrQuestion _endOrQuestion;
    private Question _nextQuestion;
    
    public MainWindow(Survey survey)
    {
        _survey = survey;
        InitializeComponent();
    }
}