using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SADA.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace SADA
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        {
            Services = ConfigureServices();

            RegisterMessages();

            this.InitializeComponent();
        }

        private void RegisterMessages()
        {
            WeakReferenceMessenger.Default.Register(Services.GetService<ViewModel.Start.MainViewModel>());
        }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => Application.Current as App;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var view = Services.GetService<View.Start.MainView>();
            view.Show();
        }


        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            ConfigureOtherServices(services);
            ConfigureView(services);

            return services.BuildServiceProvider();
        }

        private static void ConfigureView(ServiceCollection services)
        {
            services.AddSingleton<View.Start.MainView>();
            services.AddSingleton<View.Start.AuthView>();
            services.AddSingleton<View.Start.TestView>();

            services.AddSingleton<ViewModel.Start.MainViewModel>();
            services.AddSingleton<ViewModel.Start.TestViewModel>();
            services.AddSingleton<ViewModel.Start.AuthViewModel>();
            
            services.AddSingleton<INavigationService, NavigationService>();

        }

        private static void ConfigureOtherServices(ServiceCollection services)
        {
            
        }

        
    }
}
