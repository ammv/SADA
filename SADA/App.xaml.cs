using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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

            this.InitializeComponent();
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
            var view = Services.GetService<View.Start.AuthorizationView>();
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
            services.AddSingleton<View.Start.AuthorizationView>();
            services.AddSingleton<ViewModel.Start.AuthorizationViewModel>();

            services.AddSingleton(provider => new View.Start.AuthorizationView
            {
                DataContext = provider.GetRequiredService<ViewModel.Start.AuthorizationViewModel>()
            });
        }

        private static void ConfigureOtherServices(ServiceCollection services)
        {
            services.AddSingleton<Func<Type, ObservableObject>>(serviceProvider => viewModelType =>
              (ObservableObject)serviceProvider.GetRequiredService(viewModelType));
        }

        
    }
}
