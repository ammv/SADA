using DataLayer;
using FadeWpf;
using HandyControl.Themes;
using HandyControl.Tools;
using Microsoft.Extensions.DependencyInjection;
using SADA.Infastructure.Core;
using SADA.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

            ThemeManager.Current.ActualApplicationThemeChanged += Current_SystemThemeChanged;

            if (DateTime.Now.Hour > 18 || 8 > DateTime.Now.Hour)
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            }
            else
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            }

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

        public User CurrentUser { get; set; }

        public ObservableCollection<ITab> UserTabs { get; set; }

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
            ConfigHelper.Instance.SetLang("ru");

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

            Type[] typeBlackList = { 
                typeof(View.Start.AuthView),
                typeof(View.Start.LoadingView)
            };
            if (typeBlackList.Contains(windowService.LastOpenedWindow.GetType())) return;

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
            СonfigureMainMenu_Home(services);
            СonfigureMainMenu_SalaryAndStaff(services);
        }

        private void СonfigureMainMenu_SalaryAndStaff(ServiceCollection services)
        {
            СonfigureMainMenu_SalaryAndStaff_Staff(services);
            //СonfigureMainMenu_Car_Income(services);
           // СonfigureMainMenu_Car_Counteragent(services);
        }

        private void СonfigureMainMenu_SalaryAndStaff_Staff(ServiceCollection services)
        {
            services.AddTransient<ViewModel.MainMenu.SalaryAndStaff.Staff.StaffListViewModel>();
            services.AddTransient<ViewModel.MainMenu.SalaryAndStaff.Staff.StaffViewModel>();
        }

        private void СonfigureMainMenu_Home(ServiceCollection services)
        {
            СonfigureMainMenu_Home_Expense(services);
            //СonfigureMainMenu_Car_Income(services);
            СonfigureMainMenu_Car_Counteragent(services);
        }

        private void СonfigureMainMenu_Car(ServiceCollection services)
        {
            СonfigureMainMenu_Car_Salon(services);
            СonfigureMainMenu_Car_Car(services);
            
        }

        private void СonfigureMainMenu_Home_Expense(ServiceCollection services)
        {
            services.AddTransient<ViewModel.MainMenu.Home.Expense.CarExpenseListViewModel>();
            services.AddTransient<ViewModel.MainMenu.Home.Expense.CarExpenseViewModel>();
            services.AddTransient<ViewModel.MainMenu.Home.Expense.GeneralExpenseListViewModel>();
            services.AddTransient<ViewModel.MainMenu.Home.Expense.GeneralExpenseViewModel>();
        }

        private void СonfigureMainMenu_Car_Counteragent(ServiceCollection services)
        {
            services.AddTransient<ViewModel.MainMenu.Home.Counteragent.CounteragentListViewModel>();
            services.AddTransient<ViewModel.MainMenu.Home.Counteragent.CounteragentViewModel>();
            //services.AddTransient<ViewModel.MainMenu.Home.Expense.GeneralExpenseListViewModel>();
            //services.AddTransient<ViewModel.MainMenu.Home.Expense.GeneralExpenseViewModel>();
        }

        private void СonfigureMainMenu_Car_Salon(ServiceCollection services)
        {
            services.AddTransient<ViewModel.MainMenu.Car.Salon.CarInSalonListViewModel>();
            services.AddTransient<ViewModel.MainMenu.Car.Salon.CarInSalonViewModel>();
        }

        private void СonfigureMainMenu_Car_Car(ServiceCollection services)
        {
            services.AddTransient<ViewModel.MainMenu.Car.Car.PayToCounteragentListViewModel>();
            services.AddTransient<ViewModel.MainMenu.Car.Car.PayToCounteragentViewModel>();
            services.AddTransient<ViewModel.MainMenu.Car.Car.PurchaseFromCounteragentListViewModel>();
            services.AddTransient<ViewModel.MainMenu.Car.Car.PurchaseFromCounteragentViewModel>();
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
            services.AddSingleton<View.Dialogs.MainMenu.AdministrationDialogView >();
            services.AddSingleton<View.Dialogs.MainMenu.CarDialogView>();
            services.AddSingleton<View.Dialogs.MainMenu.HomeDialogView>();
            services.AddSingleton<View.Dialogs.MainMenu.ManualDialogView>();
            services.AddSingleton<View.Dialogs.MainMenu.ProductDialogView>();
            services.AddSingleton<View.Dialogs.MainMenu.SalaryAndStaffDialogView>();

            services.AddSingleton<View.Dialogs.Other.CounteragentContactPersonDialogView>();

            services.AddSingleton<ViewModel.Dialogs
                .MainMenu.AdministrationDialogViewModel>();
            services.AddSingleton<ViewModel.Dialogs
                .MainMenu.CarDialogViewModel>();
            services.AddSingleton<ViewModel.Dialogs
                .MainMenu.HomeDialogViewModel>();
            services.AddSingleton<ViewModel.Dialogs
                .MainMenu.ManualDialogViewModel>();
            services.AddSingleton<ViewModel.Dialogs
                .MainMenu.ProductDialogViewModel>();
            services.AddSingleton<ViewModel.Dialogs
                .MainMenu.SalaryAndStaffDialogViewModel>();

            services.AddSingleton<ViewModel.Dialogs.Other.CounteragentContactPersonDialogViewModel>();
        }

        private void ConfigureOtherServices(ServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ITabService, TabService>();
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