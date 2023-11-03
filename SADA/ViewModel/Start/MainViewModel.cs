using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataLayer;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SADA.ViewModel.Start
{
    class MainViewModel : ObservableObject
    {

        #region Constructor
        public MainViewModel()
        {
            //_tabItemInfos.Add(new TabItemInfo("Мурад черт",
            //    App.Current.GetService<TestViewModel>()));
            //_tabItemInfos.Add(new TabItemInfo("Мурад черт 1",
            //    App.Current.GetService<TestViewModel>()));
            //_tabItemInfos.Add(new TabItemInfo("Мурад черт 2",
            //    App.Current.GetService<TestViewModel>()));

            _menuMap.Add("Главная", typeof(TestViewModel));
            _menuMap.Add("Администрирование", typeof(TestViewModel));
            _menuMap.Add("Контрагенты", typeof(TestViewModel));
            _menuMap.Add("Продажи", typeof(TestViewModel));

            OpenTabCommand = new RelayCommand<string>(_OpenTabCommand);

            using (var ctx = new SADAEntities())
            {
                Staff = ctx.Staff.FirstOrDefault(s => s.UserID == App.CurrentUser.ID);
                MessageBox.Show(Staff.Passport.Name);
            }
        }

        #endregion

        #region Fields

        private ObservableCollection<TabItemInfo> _tabItemInfos = new ObservableCollection<TabItemInfo>();
        private int _selectedTabItemIndex;
        private Dictionary<string, Type> _menuMap = new Dictionary<string, Type>();

        #endregion

        #region Properties

        public ObservableCollection<TabItemInfo> TabItemInfos
        {
            get => _tabItemInfos;
            set => SetProperty(ref _tabItemInfos, value);
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


        #endregion

        #region Commands implementations

        private void _OpenTabCommand(string parameter)
        {

            //1. Понять, открыта ли уже текущая вкладка
            int index = -1;
            for (int i = 0; i < _tabItemInfos.Count; i++)
            {
                if (_tabItemInfos[i].Name == parameter)
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
                if (_menuMap.TryGetValue(parameter, out Type viewModelType))
                {
                    _tabItemInfos.Add(new TabItemInfo(parameter,
                    App.Current.Services.GetService(_menuMap[parameter]) as ObservableObject));
                }

            }

        }

        #endregion

        #region Other

        public class TabItemInfo
        {
            public TabItemInfo(string name, ObservableObject viewModel)
            {
                Name = name;
                ViewModel = viewModel;
                GUID = Guid.NewGuid();
            }

            public string Name { get; set; }
            public Guid GUID { get; }
            public ObservableObject ViewModel { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        #endregion
    }
}
