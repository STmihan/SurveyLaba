using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfCreateSRV;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MainCanvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var rectangle = new Rectangle
        {
            Width = 40,
            Height = 40,
            Fill = Brushes.Red,
        };
            
        Canvas.SetLeft(rectangle, e.GetPosition(MainCanvas).X);
        Canvas.SetTop(rectangle, e.GetPosition(MainCanvas).Y);
            
        MainCanvas.Children.Add(rectangle);
        Console.WriteLine(rectangle);
    }
}