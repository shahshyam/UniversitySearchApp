using GlobalUniversityApp.Client;
using GlobalUniversityApp.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;


namespace GlobalUniversityApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            host = Host.CreateDefaultBuilder()
                  .ConfigureServices((context, services) =>
                   {
                       ConfigureServices(context.Configuration, services);
                   })
                   .Build();
            ServiceProvider = host.Services;
        }
        private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddScoped<IUniversityClient, UniversityClient>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<MainWindow>();
        }
        protected override async void OnStartup( StartupEventArgs e)
        {
            await host.StartAsync();
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
