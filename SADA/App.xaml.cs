using DataLayer;
using FadeWpf;
using HandyControl.Tools;
using Microsoft.Extensions.DependencyInjection;
using SADA.Services;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace SADA
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static User CurrentUser { get; set; }
        public App()
        {
            Services = ConfigureServices();

            RegisterMessages();

            this.InitializeComponent();
        }

        private void RegisterMessages()
        {
            //WeakReferenceMessenger.Default.Register(Services.GetService<ViewModel.Start.MainViewModel>());
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
            var view = Services.GetService<View.Start.AuthView>();
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

            services.AddSingleton<ViewModel.Start.MainViewModel>();
            services.AddSingleton<ViewModel.Start.AuthViewModel>();
            services.AddTransient<ViewModel.Start.TestViewModel>();
        }

        private static void ConfigureOtherServices(ServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<WindowFadeChanger>();
            services.AddSingleton<IDataGridService, DataGridService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IDatabaseTableService>(
                provider => new DatabaseTableService(new SADAEntities()));
        }

        public T GetService<T>()
        {
            return Services.GetService<T>();
        }

    }
}
