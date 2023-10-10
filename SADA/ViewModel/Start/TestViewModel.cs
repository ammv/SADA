using CommunityToolkit.Mvvm.Input;
using SADA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SADA.ViewModel.Start
{
    class TestViewModel
    {

        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Commands

        public ICommand BackCommand { get; }

        private INavigationService _navigationService;

        #endregion

        #region Command implementations

        private void _BackCommand()
        {
            _navigationService.NavigateTo<View.Start.AuthView>();
        }

        #endregion

        #region Конструктор
        public TestViewModel(INavigationService navigationService)
        {
            BackCommand = new RelayCommand(_BackCommand);
            _navigationService = navigationService;
        }

        #endregion
    }


}
