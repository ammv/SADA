using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SADA.ViewModel.Start
{
    class MainViewModel : ObservableObject
    {

        #region Constructor
        public MainViewModel()
        {
            _tabItemInfos.Add(new TabItemInfo( "Мурад черт",
                App.Current.GetService<TestViewModel>()));
            _tabItemInfos.Add(new TabItemInfo("Мурад черт 1",
                App.Current.GetService<TestViewModel>()));
            _tabItemInfos.Add(new TabItemInfo("Мурад черт 2",
                App.Current.GetService<TestViewModel>()));
        }

        #endregion

        #region Fields

        private ObservableCollection<TabItemInfo> _tabItemInfos = new ObservableCollection<TabItemInfo>();

        #endregion

        #region Properties

        public ObservableCollection<TabItemInfo> TabItemInfos
        {
            get => _tabItemInfos;
            set => SetProperty(ref _tabItemInfos, value);
        }

        #endregion

        #region Commands


        #endregion

        #region Commands implementations

        #endregion

        #region Other

        public class TabItemInfo
        {
            public TabItemInfo(string name, ObservableObject viewModel)
            {
                Name = name;
                ViewModel = viewModel;
            }

            public string Name { get; set; }
            public ObservableObject ViewModel { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        #endregion
    }
}
