using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataLayer;
using SADA.Infastructure.Core.Enums;
using SADA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.ViewModel.Dialogs.Other
{
    /// <summary>
    /// Возвращает новую сущность или измененную
    /// </summary>
    public class CounteragentContactPersonDialogViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        #region Fields

        private FormMode _currentFormMode = FormMode.NotSet;
        private CounteragentContactPerson _contactPerson;

        private Action<CounteragentContactPerson> _addAction;
        private Action<CounteragentContactPerson> _editAction;

        private IEnumerable<StaffRole> _staffRoles;
        private IEnumerable<StaffPost> _staffPosts;

        #endregion

        #region Constructor

        public CounteragentContactPersonDialogViewModel(IDialogService dialogService)
        {
            ConfirmCommand = new RelayCommand(_ConfirmCommand);
            _dialogService = dialogService;
        }
        #endregion

        #region Properties

        public FormMode CurrentFormMode
        {
            get => _currentFormMode;
            set
            {
                if (SetProperty(ref _currentFormMode, value))
                {
                    OnPropertyChanged(nameof(FormModeHeader));
                }
            }

        }

        public CounteragentContactPerson ContactPerson
        {
            get => _contactPerson;
            set => SetProperty(ref _contactPerson, value);
        }

        public Action<CounteragentContactPerson> AddAction
        {
            get => _addAction;
            set => SetProperty(ref _addAction, value);
        }

        public IEnumerable<StaffRole> StaffRoles
        {
            get => _staffRoles;
            set => SetProperty(ref _staffRoles, value);
        }

        public IEnumerable<StaffPost> StaffPosts
        {
            get => _staffPosts;
            set => SetProperty(ref _staffPosts, value);
        }

        public Action<CounteragentContactPerson> EditAction
        {
            get => _editAction;
            set => SetProperty(ref _editAction, value);
        }

        public string FormModeHeader
        {
            get
            {
                switch (_currentFormMode)
                {
                    case FormMode.Add:
                        return "Добавление контактного лица";
                    case FormMode.Edit:
                        return "Изменение контактного лица";
                }
                return "Неизвестная операция";
            }
        }
        #endregion

        #region Commands

        public RelayCommand ConfirmCommand { get; }
        #endregion

        #region Command implementation
        private void _ConfirmCommand()
        {
            string[] necessary = { _contactPerson.Name, _contactPerson.Surname };
            if(necessary.Any(c => string.IsNullOrEmpty(c)))
            {
                _dialogService.ShowMessageBox("Ошибка", "Вы не заполнили все поля", System.Windows.MessageBoxButton.OK);
                return;
            }
            switch(_currentFormMode)
            {
                case FormMode.Add:
                    _addAction?.Invoke(_contactPerson);
                    return;
                case FormMode.Edit:
                    _editAction?.Invoke(_contactPerson);
                    return;
            }
        }
        #endregion

        #region Other
        #endregion

    }
}
