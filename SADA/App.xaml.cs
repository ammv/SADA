﻿using CommunityToolkit.Mvvm.Messaging;
using DataLayer;
using FadeWpf;
using HandyControl.Tools;
using Microsoft.Extensions.DependencyInjection;
using SADA.Infastructure.Messages;
using SADA.Services;
using SADA.ViewModel.Start;
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
        System.Threading.Mutex mutex;
        public static User CurrentUser { get; set; }
        public App()
        {
            Services = ConfigureServices();

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            ShutdownMode = ShutdownMode.OnLastWindowClose;

            RegisterMessages();

            this.InitializeComponent();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Непредвиденная ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void RegisterMessages()
        {
            //WeakReferenceMessenger.Default.Register<MainViewModel, OpenTabFromDialogMessage>();
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
            bool createdNew;
            string mutName = "Приложение";
            mutex = new System.Threading.Mutex(true, mutName, out createdNew);
            if (!createdNew)
            {
                this.Shutdown();
            }
            var view = Services.GetService<View.Start.MainView>();
            view.Show();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            ConfigureOtherServices(services);
            ConfigureView(services);
            ConfigureViewModel(services);

            return services.BuildServiceProvider();
        }

        private static void ConfigureView(ServiceCollection services)
        {
            services.AddTransient<View.Start.MainView>();
            services.AddTransient<View.Start.AuthView>();
            services.AddSingleton<Infastructure.Dialogs.View.MenuDialogView>();

        }

        private static void ConfigureViewModel(ServiceCollection services)
        {
            services.AddTransient<ViewModel.Start.MainViewModel>();
            services.AddTransient<ViewModel.Start.AuthViewModel>();
            services.AddTransient<ViewModel.Start.TestViewModel>();


            services.AddSingleton<Infastructure.Dialogs.View.MainMenu.AdministrationDialogView>();
            services.AddSingleton<Infastructure.Dialogs.View.MainMenu.CarDialogView>();
            services.AddSingleton<Infastructure.Dialogs.View.MainMenu.HomeDialogView>();
            services.AddSingleton<Infastructure.Dialogs.View.MainMenu.ManualDialogView>();
            services.AddSingleton<Infastructure.Dialogs.View.MainMenu.ProductDialogView>();
            services.AddSingleton<Infastructure.Dialogs.View.MainMenu.SalaryAndStaffDialogView>();
        }

        private static void ConfigureOtherServices(ServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<WindowFadeChanger>();
            //services.AddSingleton<IDataGridService, DataGridService>();
            services.AddSingleton<INavigationService, NavigationService>();
            //services.AddSingleton<IDatabaseTableService>(
            //    provider => new DatabaseTableService(new SADAEntities()));
        }

        public T GetService<T>()
        {
            return Services.GetService<T>();
        }

        public Window LastOpenedWindow()
        {
            return Windows[Windows.Count - 1];
        }

    }
}
