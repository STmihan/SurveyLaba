using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WpfCreateSRV.Services;
using WpfCreateSRV.Views.Controls;

namespace WpfCreateSRV;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IServiceProvider ServiceProvider { get; private set; }
    
    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        
        ServiceProvider = serviceCollection.BuildServiceProvider();
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
    
    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddSingleton<ConstructSurveyService>();
        services.AddSingleton<SurveyCanvasUserControl>();
    }
}
