using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataLayer;
using FadeWpf;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Themes;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.Services;
using SADA.View.Start;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using MessageBox = HandyControl.Controls.MessageBox;
using Window = System.Windows.Window;

namespace SADA.ViewModel.Start
{
    internal partial class MainViewModel : ObservableObject, IRecipient<DialogTabChangedMessage>
    {
        #region Fields

        private ObservableCollection<ITab> _Tabs = new ObservableCollection<ITab>();
        private int _selectedTabItemIndex = -1;
        private readonly WindowFadeChanger _windowFadeChanger;
        private readonly IWindowService _windowService;
        private readonly IDialogService _dialogService;
        private Dialog _currentDialog = null;
        private const double _sideMenuWidthBase = 240;
        private const double _sideMenuWidthMinimum = 50;
        private double _sideMenuWidth = _sideMenuWidthBase;
        private Dictionary<string, Type> _dialogMappings;

        #endregion Fields

        #region Constructor

        public MainViewModel(WindowFadeChanger windowFadeChanger, IWindowService windowService, IDialogService dialogService)
        {
            _windowFadeChanger = windowFadeChanger;
            _windowService = windowService;
            _dialogService = dialogService;
            OpenTabCommand = new RelayCommand<string>(_OpenDialogCommand);
            OpenCalculatorToolCommand = new RelayCommand(_OpenCalculatorToolCommand);
            ExitFromAccountCommand = new RelayCommand<Window>(_ExitFromAccountCommand);
            CloseSelectedTabCommand = new RelayCommand(_CloseSelectedTabCommand);
            ChangeSideMenuWidthCommand = new RelayCommand(_ChangeSideMenuWidthCommand);
            ChangeThemeCommand = new RelayCommand(_ChangeThemeCommand);

            _Tabs.CollectionChanged += _Tabs_CollectionChanged;

            _Tabs.Add(App.Current.GetService<WelcomeTabViewModel>());

            using (var ctx = new SADAEntities())
            {
                //Staff = ctx.Staff.FirstOrDefault(s => s.UserID == App.CurrentUser.ID);
                Staff = ctx.Staff.Include("Passport").Include("StaffPost").FirstOrDefault(s => s.UserID == 1);
            }

            _dialogMappings = new Dictionary<string, Type>
            {
                { "Главное", typeof(Infastructure.Dialogs.View.MainMenu.HomeDialogView) },
                { "Администрирование", typeof(Infastructure.Dialogs.View.MainMenu.AdministrationDialogView) },
                { "Автомобили", typeof(Infastructure.Dialogs.View.MainMenu.CarDialogView) },
                { "Зарплата и кадры", typeof(Infastructure.Dialogs.View.MainMenu.SalaryAndStaffDialogView) },
                { "Справочники", typeof(Infastructure.Dialogs.View.MainMenu.ManualDialogView) },
                { "Товары", typeof(Infastructure.Dialogs.View.MainMenu.ProductDialogView) }
            };

            WeakReferenceMessenger.Default.Register(this);
        }

        // For Mock
        protected MainViewModel()
        {

        }

        #endregion Constructor

        #region Properties

        public ObservableCollection<ITab> Tabs
        {
            get => _Tabs;
            set => SetProperty(ref _Tabs, value);
        }

        public int SelectedTabItemIndex
        {
            get => _selectedTabItemIndex;
            //set => SetProperty(ref _selectedTabItemIndex, value == -1 ? _Tabs.Count - 1: value);
            set => SetProperty(ref _selectedTabItemIndex, value);
        }

        public double SideMenuWidth
        {
            get => _sideMenuWidth;
            set => SetProperty(ref _sideMenuWidth, value);
        }

        public double SideMenuWidthMinimum
        {
            get => _sideMenuWidth;
            set => SetProperty(ref _sideMenuWidth, value);
        }

        public double SideMenuWidthDefault
        {
            get => _sideMenuWidthBase;
        }

        public Staff Staff { get; }

        #endregion Properties

        #region Commands

        public RelayCommand<string> OpenTabCommand { get; }
        public RelayCommand<Window> ExitFromAccountCommand { get; }
        public RelayCommand CloseSelectedTabCommand { get; }
        public RelayCommand ChangeSideMenuWidthCommand { get; }
        public RelayCommand ChangeThemeCommand { get; }
        public RelayCommand OpenCalculatorToolCommand { get; }

        #endregion Commands

        #region Commands implementations

        private void _ChangeThemeCommand()
        {
            ThemeManager.Current.ApplicationTheme = ThemeManager.Current.ApplicationTheme == ApplicationTheme.Light ? ApplicationTheme.Dark : ApplicationTheme.Light;
        }

        private void _OpenDialogCommand(string parameter)
        {
            if (_dialogMappings.TryGetValue(parameter, out Type dialogType))
            {
                UserControl uc = App.Current.Services.GetService(dialogType) as UserControl;
                _currentDialog = Dialog.Show(uc);
            }
        }

        private void _CloseSelectedTabCommand()
        {
            if (_Tabs.Count == 0)
            {
                return;
            }
            _Tabs[_selectedTabItemIndex].CloseCommand?.Execute(null);
        }

        private void _OpenCalculatorToolCommand()
        {
            Process.Start("calc.exe");
        }

        private void _ChangeSideMenuWidthCommand()
        {
            if (_sideMenuWidth > _sideMenuWidthBase)
            {
                SideMenuWidth = _sideMenuWidthBase;
            }
            else if (_sideMenuWidth == _sideMenuWidthBase)
            {
                SideMenuWidth = _sideMenuWidthMinimum;
            }
            else
            {
                SideMenuWidth = _sideMenuWidthBase;
            }
        }

        private void _ExitFromAccountCommand(Window window)
        {
            var result = _dialogService.ShowMessageBox("Вопрос", "Вы уверены что хотите выйти из системы?", System.Windows.MessageBoxButton.YesNo);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                _windowService.ShowAndCloseWindow<AuthView>(_windowService.LastOpenedWindow);
                
            }
        }

        #endregion Commands implementations

        #region Other

        public void Receive(DialogTabChangedMessage message)
        {
            if (_currentDialog != null)
            {
                Tabs.Add(message.Value);
                SelectedTabItemIndex = Tabs.Count - 1;
                _currentDialog.Close();
            }
        }

        private void _Tabs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ITab tab;

            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    tab = (ITab)e.NewItems[0];
                    tab.CloseRequested += Tab_CloseRequested;
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    tab = (ITab)e.OldItems[0];
                    tab.CloseRequested -= Tab_CloseRequested;
                    break;
            }
        }

        private void Tab_CloseRequested(object sender, EventArgs e)
        {
            //int currentTabCount = _Tabs.Count;
            Tabs.Remove((ITab)sender);
            //if(Sele)
            //SelectedTabItemIndex = 0;
            

            //if(SelectedTabItemIndex = )
            
        }

        #endregion Other
    }
}
