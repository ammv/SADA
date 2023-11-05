using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataLayer;
using FadeWpf;
using HandyControl.Data;
using SADA.Infastructure.Core;
using SADA.View.Start;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace SADA.ViewModel.Start
{
    partial class MainViewModel : ObservableObject
    {

        #region Constructor
        public MainViewModel(WindowFadeChanger windowFadeChanger = null)
        {
            _menuMap.Add("Главная", TypeWrapper<TabObservableObject>.Make<TestViewModel>());
            _menuMap.Add("Администрирование", TypeWrapper<TabObservableObject>.Make<TestViewModel>());
            _menuMap.Add("Контрагенты", TypeWrapper<TabObservableObject>.Make<TestViewModel>());
            _menuMap.Add("Продажи", TypeWrapper<TabObservableObject>.Make<TestViewModel>());

            OpenTabCommand = new RelayCommand<string>(_OpenTabCommand);
            OpenCalculatorToolCommand = new RelayCommand(_OpenCalculatorToolCommand);
            ExitFromAccountCommand = new RelayCommand<Window>(_ExitFromAccountCommand);

            _Tabs.CollectionChanged += _Tabs_CollectionChanged;

            using (var ctx = new SADAEntities())
            {
                //Staff = ctx.Staff.FirstOrDefault(s => s.UserID == App.CurrentUser.ID);
                Staff = ctx.Staff.FirstOrDefault(s => s.UserID == 1);
                MessageBox.Show(Staff.Passport.Name);
            }

            this._windowFadeChanger = windowFadeChanger;


            /*
             * TypeRestrictor<TabObservableObject>
             * TypeWrapper<T, TypeRestrictor<TRestrict>>
             * 
             * 
             */
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

        private void _OpenTabCommand(string parameter)
        {

            //1. Понять, открыта ли уже текущая вкладка
            int index = -1;
            for (int i = 0; i < _Tabs.Count; i++)
            {
                if (_Tabs[i].Name == parameter)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                SelectedTabItemIndex = index;
            }
            else
            {
                if (_menuMap.TryGetValue(parameter, out TypeWrapper<TabObservableObject> typeWrapper))
                {
                    var tabVm = App.Current.Services.GetService(typeWrapper.TypeDerived) as TabObservableObject;
                    tabVm.Name = parameter;
                    Tabs.Add(tabVm);

                    SelectedTabItemIndex = _Tabs.Count - 1;
                }
            }
        }

        private void _OpenCalculatorToolCommand()
        {
            Process.Start("calc.exe");
        }

        private void _ExitFromAccountCommand(Window window)
        {
            var result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
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

        #endregion
        #region Other

        #endregion
    }
}
