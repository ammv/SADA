using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataLayer;
using FadeWpf;
using HandyControl.Controls;
using HandyControl.Data;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.View.Start;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using MessageBox = HandyControl.Controls.MessageBox;
using Window = System.Windows.Window;

namespace SADA.ViewModel.Start
{
    internal partial class MainViewModel : ObservableObject, IRecipient<DialogTabChangedMessage>
    {
        #region Constructor

        public MainViewModel(WindowFadeChanger windowFadeChanger = null)
        {
            this._windowFadeChanger = windowFadeChanger;

            //OpenTabCommand = new RelayCommand<string>(_OpenTabCommand);
            OpenTabCommand = new RelayCommand<string>(_OpenDialogCommand);
            OpenCalculatorToolCommand = new RelayCommand(_OpenCalculatorToolCommand);
            ExitFromAccountCommand = new RelayCommand<Window>(_ExitFromAccountCommand);
            CloseSelectedTabCommand = new RelayCommand(_CloseSelectedTabCommand);
            ChangeSideMenuWidth = new RelayCommand(_ChangeSideMenuWidth);

            _Tabs.CollectionChanged += _Tabs_CollectionChanged;

            _Tabs.Add(App.Current.GetService<WelcomeTabViewModel>());

            using (var ctx = new SADAEntities())
            {
                //Staff = ctx.Staff.FirstOrDefault(s => s.UserID == App.CurrentUser.ID);
                Staff = ctx.Staff.Include("Passport").Include("StaffPost").FirstOrDefault(s => s.UserID == 1);
            }

            WeakReferenceMessenger.Default.Register(this);
        }

        private void Tab_CloseRequested(object sender, EventArgs e)
        {
            Tabs.Remove((ITab)sender);
        }

        #endregion Constructor

        #region Fields

        private ObservableCollection<ITab> _Tabs = new ObservableCollection<ITab>();
        private int _selectedTabItemIndex;
        private Dictionary<string, TypeWrapper<TabObservableObject>> _menuMap = new Dictionary<string, TypeWrapper<TabObservableObject>>();
        private readonly WindowFadeChanger _windowFadeChanger;
        private Dialog _currentDialog = null;
        private const double _sideMenuWidthBase = 240;
        private const double _sideMenuWidthMinimum = 50;
        private double _sideMenuWidth = _sideMenuWidthBase;

        #endregion Fields

        #region Properties

        public ObservableCollection<ITab> Tabs
        {
            get => _Tabs;
            set => SetProperty(ref _Tabs, value);
        }

        public int SelectedTabItemIndex
        {
            get => _selectedTabItemIndex;
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

        public RelayCommand OpenCalculatorToolCommand { get; }
        public RelayCommand<Window> ExitFromAccountCommand { get; }
        public RelayCommand CloseSelectedTabCommand { get; }
        public RelayCommand ChangeSideMenuWidth { get; }

        #endregion Commands

        #region Commands implementations

        private void _OpenDialogCommand(string parameter)
        {
            //var dialog = ;
            switch (parameter)
            {
                case "Главное":
                    _currentDialog = Dialog.Show(App.Current.GetService<Infastructure.Dialogs.View.MainMenu.HomeDialogView>());
                    break;

                case "Администрирование":
                    _currentDialog = Dialog.Show(App.Current.GetService<Infastructure.Dialogs.View.MainMenu.AdministrationDialogView>());
                    break;

                case "Автомобили":
                    _currentDialog = Dialog.Show(App.Current.GetService<Infastructure.Dialogs.View.MainMenu.CarDialogView>());
                    break;

                case "Зарплата и кадры":
                    _currentDialog = Dialog.Show(App.Current.GetService<Infastructure.Dialogs.View.MainMenu.SalaryAndStaffDialogView>());
                    break;

                case "Справочники":
                    _currentDialog = Dialog.Show(App.Current.GetService<Infastructure.Dialogs.View.MainMenu.ManualDialogView>());
                    break;

                case "Товары":
                    _currentDialog = Dialog.Show(App.Current.GetService<Infastructure.Dialogs.View.MainMenu.ProductDialogView>());
                    break;
            }

            // result.InputBindings.AddRange(dialog.InputBindings);
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

        private void _ChangeSideMenuWidth()
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
            var result = MessageBox.Show(new MessageBoxInfo
            {
                Message = "Вы уверены, что хотите выйти из аккаунта?",
                Caption = "Выход из аккаунта",
                Button = System.Windows.MessageBoxButton.YesNo,
                YesContent = "Да",
                NoContent = "Нет"
            });

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                _windowFadeChanger.Change(window, App.Current.GetService<AuthView>());
            }
        }

        #endregion Commands implementations

        #region Other

        public void Receive(DialogTabChangedMessage message)
        {
            if (_currentDialog != null)
            {
                Tabs.Add(message.Value);
                //SelectedTabItemIndex = Tabs.Count - 1;
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
                    SelectedTabItemIndex = _Tabs.Count - 1;

                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    tab = (ITab)e.OldItems[0];
                    tab.CloseRequested -= Tab_CloseRequested;
                    //if(SelectedTabItemIndex == _Tabs.Count - 2)
                    break;
            }
        }

        #endregion Other
    }
}