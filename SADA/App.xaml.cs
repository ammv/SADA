using DataLayer;
using FadeWpf;
using HandyControl.Themes;
using Microsoft.Extensions.DependencyInjection;
using SADA.Infastructure.Core;
using SADA.Services;
using System;
using System.Windows;
using WpfUtils;

namespace SADA
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        #region Fields

        private System.Threading.Mutex mutex;

        private IdleDetector _idleDetector = new IdleDetector(TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(1));

        #endregion Fields

        #region Constructor

        public App()
        {
            this.InitializeComponent();

            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;

            ThemeManager.Current.ActualApplicationThemeChanged += Current_SystemThemeChanged;

            Services = ConfigureServices();
        }

        private void Current_SystemThemeChanged(ThemeManager themeManager, object @object)
        {
            if(themeManager.ActualApplicationTheme == ApplicationTheme.Dark)
            {
                Application.Current.Resources["CurrentThemeIcon"] = Application.Current.Resources["SunIcon"];
            }
            else
            {
                Application.Current.Resources["CurrentThemeIcon"] = Application.Current.Resources["MoonIcon"];
            }
            
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

            _idleDetector.IdleDetect += IdleDetect_Handler;

            new DataTemplateManager().RegisterDataTemplateAuto();

            RegisterMessages();

            ShutdownAppCopy();

            IWindowService windowService = Services.GetService<IWindowService>();

            windowService.ShowWindow<View.Start.LoadingView>();

            _idleDetector.Start();
        }

        private void IdleDetect_Handler(IdleDetector sender, IdleTimeInfo idleTimeInfo)
        {
            IWindowService windowService = Services.GetService<IWindowService>();
            IDialogService dialogService = Services.GetService<IDialogService>();

            if (windowService.LastOpenedWindow.GetType() == typeof(View.Start.AuthView)) return;

            windowService.ShowAndCloseWindow<View.Start.AuthView>(windowService.LastOpenedWindow);

            dialogService.ShowMessageBox("Сессия прекращена",
                $"Сессия прекращена из за длительного бездействия в течении {idleTimeInfo.IdleTime}",
                MessageBoxButton.OK);
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
            СonfigureMainMenu(services);

            return services.BuildServiceProvider();
        }

        private void СonfigureMainMenu(ServiceCollection services)
        {
            СonfigureMainMenu_Car(services);
        }

        private void СonfigureMainMenu_Car(ServiceCollection services)
        {
            СonfigureMainMenu_Car_Salon(services);
            
        }

        private void СonfigureMainMenu_Car_Salon(ServiceCollection services)
        {
            services.AddTransient<ViewModel.MainMenu.Car.Salon.CarInSalonViewModel>();
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