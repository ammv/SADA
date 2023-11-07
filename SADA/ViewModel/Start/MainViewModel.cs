using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataLayer;
using FadeWpf;
using HandyControl.Controls;
using HandyControl.Data;
using SADA.Infastructure.Core;
using SADA.Infastructure.Dialogs;
using SADA.Infastructure.Dialogs.View;
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
    partial class MainViewModel : ObservableObject, IRecipient<DialogTabChangedMessage>
    {

        #region Constructor
        public MainViewModel(WindowFadeChanger windowFadeChanger = null)
        {
            this._windowFadeChanger = windowFadeChanger;

            //OpenTabCommand = new RelayCommand<string>(_OpenTabCommand);
            OpenTabCommand = new RelayCommand<string>(_OpenDialogCommand);
            OpenCalculatorToolCommand = new RelayCommand(_OpenCalculatorToolCommand);
            ExitFromAccountCommand = new RelayCommand<Window>(_ExitFromAccountCommand);

            _Tabs.CollectionChanged += _Tabs_CollectionChanged;

            using (var ctx = new SADAEntities())
            {
                //Staff = ctx.Staff.FirstOrDefault(s => s.UserID == App.CurrentUser.ID);
                Staff = ctx.Staff.Include("Passport").Include("StaffPost").FirstOrDefault(s => s.UserID == 1);
            }

            WeakReferenceMessenger.Default.Register(this);

        }

        private void _Tabs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ITab tab;

            switch(e.Action)
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
            Tabs.Remove((ITab)sender);
        }

        #endregion

        #region Fields

        private ObservableCollection<ITab> _Tabs = new ObservableCollection<ITab>();
        private int _selectedTabItemIndex;
        private Dictionary<string, TypeWrapper<TabObservableObject>> _menuMap = new Dictionary<string, TypeWrapper<TabObservableObject>>();
        private readonly WindowFadeChanger _windowFadeChanger;
        private Dialog _currentDialog = null;

        #endregion

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

        public Staff Staff { get; }

        #endregion

        #region Commands

        public RelayCommand<string> OpenTabCommand { get; }

        public RelayCommand OpenCalculatorToolCommand { get; }
        public RelayCommand<Window> ExitFromAccountCommand { get; }


        #endregion

        #region Commands implementations

        //private void _OpenTabCommand(string parameter)
        //{

        //    //1. Понять, открыта ли уже текущая вкладка
        //    int index = -1;
        //    for (int i = 0; i < _Tabs.Count; i++)
        //    {
        //        if (_Tabs[i].Name == parameter)
        //        {
        //            index = i;
        //            break;
        //        }
        //    }

        //    if (index != -1)
        //    {
        //        SelectedTabItemIndex = index;
        //    }
        //    else
        //    {
        //        if (_menuMap.TryGetValue(parameter, out TypeWrapper<TabObservableObject> typeWrapper))
        //        {
        //            var tabVm = App.Current.Services.GetService(typeWrapper.TypeDerived) as TabObservableObject;
        //            tabVm.Name = parameter;
        //            Tabs.Add(tabVm);

        //            SelectedTabItemIndex = _Tabs.Count - 1;
        //        }
        //    }
        //}

        private void _OpenDialogCommand(string parameter)
        {
            //var dialog = ;
            _currentDialog = Dialog.Show(App.Current.GetService<MenuDialogView>());
           // result.InputBindings.AddRange(dialog.InputBindings);
        }

        private void _OpenCalculatorToolCommand()
        {
            Process.Start("calc.exe");
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

        public void Receive(DialogTabChangedMessage message)
        {
            if(_currentDialog != null)
            {
                Tabs.Add(message.Value);
                _currentDialog.Close();
            }
        }

        #endregion
        #region Other

        #endregion
    }
}
