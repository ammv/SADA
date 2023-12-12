using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SADA.Services;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace SADA.Infastructure.Core
{
    public class DialogBase : ObservableObject
    {
        #region Fields
        protected readonly ITabService _tabService;
        private List<NavigationGroup> _navigationGroups = new List<NavigationGroup>();
        #endregion

        #region Constructor

        public DialogBase(ITabService tabService)
        {
            _tabService = tabService;
            LoadedCommand = new RelayCommand<UserControl>(_loadedCommand);
        }

        #endregion

        #region Properties

        public List<NavigationGroup> NavigationGroups
        {
            get => _navigationGroups;
            set => SetProperty(ref _navigationGroups, value);
        }

        #endregion

        #region Commands

        public RelayCommand<UserControl> LoadedCommand { get; }

        #endregion

        #region Command implementation

        private void _loadedCommand(UserControl uc)
        {
            if (uc == null) return;
            uc.Focusable = true;
            Keyboard.Focus(uc);
        }

        #endregion

        #region Other

        #endregion

    }
}