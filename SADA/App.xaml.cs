using DataLayer;
using FadeWpf;
using Microsoft.Extensions.DependencyInjection;
using SADA.Infastructure.Core;
using SADA.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace SADA
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        #region Fields

        private System.Threading.Mutex mutex;

        #endregion Fields

        #region Constructor

        public App()
        {
            this.InitializeComponent();

            Services = ConfigureServices();

            Idle();
        }

        #endregion Constructor

        #region Properties

        public static User CurrentUser { get; set; }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => Application.Current as App;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        #endregion Properties

        #region Other

        private void RegisterMessages()
        {
            //WeakReferenceMessenger.Default.Register<MainViewModel, OpenTabFromDialogMessage>();
        }

        #endregion Other

        #region App

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Непредвиденная ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //base.OnStartup(e);

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            ShutdownMode = ShutdownMode.OnLastWindowClose;

            new DataTemplateManager().RegisterDataTemplateAuto();

            RegisterMessages();

            ShutdownAppCopy();
            
            IWindowService windowService = Services.GetService<IWindowService>();

            windowService.ShowWindow<View.Start.MainView>();
        }

        DispatcherTimer mIdle;
        private const long cIdleSeconds = 3;
        private void Idle()
        {
            InputManager.Current.PreProcessInput += Current_PreProcessInput;
            mIdle = new DispatcherTimer();
            //mIdle.Interval = new TimeSpan(cIdleSeconds * 1000 * 10000);
            mIdle.Interval = TimeSpan.FromSeconds(5);
            mIdle.IsEnabled = true;
            mIdle.Tick += Idle_Tick;
        }

        private void Idle_Tick(object sender, EventArgs e)
        {
            IWindowService windowService = Services.GetService<IWindowService>();
            IDialogService dialogService = Services.GetService<IDialogService>();

            if (windowService.LastOpenedWindow.GetType() == typeof(View.Start.AuthView)) return;

            windowService.ShowAndCloseWindow<View.Start.AuthView>(windowService.LastOpenedWindow);


            dialogService.ShowMessageBox("Сессия", "Сессия прекращена из за длительного бездействия", MessageBoxButton.OK);
        }

        private void Current_PreProcessInput(object sender, PreProcessInputEventArgs e)
        {
            mIdle.IsEnabled = false;
            mIdle.IsEnabled = true;
        }

        private void ShutdownAppCopy()
        {
            bool createdNew;
            string mutName = "Приложение";
            mutex = new System.Threading.Mutex(true, mutName, out createdNew);
            if (!createdNew)
            {
                this.Shutdown();
            }
        }

        #endregion App

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            ConfigureOtherServices(services);
            ConfigureStart(services);
            ConfigureDialogs(services);
            ConfigureUtils(services);

            return services.BuildServiceProvider();
        }

        private void ConfigureStart(ServiceCollection services)
        {
            services.AddTransient<ViewModel.Start.MainViewModel>();
            services.AddTransient<ViewModel.Start.AuthViewModel>();
            services.AddTransient<ViewModel.Start.TestViewModel>();
            services.AddTransient<ViewModel.Start.WelcomeTabViewModel>();
        }

        private void ConfigureUtils(ServiceCollection services)
        {
            services.AddTransient<ViewModel.Utils.WindowTopButtonsViewModel>();
        }

        private void ConfigureDialogs(ServiceCollection services)
        {
            services.AddSingleton<Infastructure.Dialogs.View.MainMenu.AdministrationDialogView>();
            services.AddSingleton<Infastructure.Dialogs.View.MainMenu.CarDialogView>();
            services.AddSingleton<Infastructure.Dialogs.View.MainMenu.HomeDialogView>();
            services.AddSingleton<Infastructure.Dialogs.View.MainMenu.ManualDialogView>();
            services.AddSingleton<Infastructure.Dialogs.View.MainMenu.ProductDialogView>();
            services.AddSingleton<Infastructure.Dialogs.View.MainMenu.SalaryAndStaffDialogView>();

            services.AddSingleton<Infastructure.Dialogs.ViewModel
                .MainMenu.AdministrationDialogViewModel>();
            services.AddSingleton<Infastructure.Dialogs.ViewModel
                .MainMenu.CarDialogViewModel>();
            services.AddSingleton<Infastructure.Dialogs.ViewModel
                .MainMenu.HomeDialogViewModel>();
            services.AddSingleton<Infastructure.Dialogs.ViewModel
                .MainMenu.ManualDialogViewModel>();
            services.AddSingleton<Infastructure.Dialogs.ViewModel
                .MainMenu.ProductDialogViewModel>();
            services.AddSingleton<Infastructure.Dialogs.ViewModel
                .MainMenu.SalaryAndStaffDialogViewModel>();
        }

        private void ConfigureOtherServices(ServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IWindowService, WindowService>();
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