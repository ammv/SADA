using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SADA.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SADA.ViewModel.Start
{
    public class MainViewModel_Old : ObservableObject
    {
        #region Constructor

        public MainViewModel_Old(IDatabaseTableService databaseTableService, IDataGridService dataGridService)
        {
            _databaseTableService = databaseTableService;
            _dataGridService = dataGridService;
            FindTableCommand = new RelayCommand<string>(_FindTableCommand);
            LoadTablePropertiesMap();
        }

        #endregion Constructor

        #region Fields

        private IDatabaseTableService _databaseTableService;
        private bool _findTableCommandIsEnabled = true;
        private readonly IDataGridService _dataGridService;
        private DataTable _datatable = new DataTable();
        private ObservableCollection<DataGridColumn> _columnCollection = new ObservableCollection<DataGridColumn>();
        private Dictionary<string, Dictionary<string, string>> _tablePropertiesMap = new Dictionary<string, Dictionary<string, string>>();

        #endregion Fields

        #region Properties

        public ObservableCollection<DataGridColumn> ColumnCollection
        {
            get => _columnCollection;
            set => SetProperty(ref _columnCollection, value);
        }

        public DataTable DataTable
        {
            get => _datatable;
            set => SetProperty(ref _datatable, value);
        }

        public bool FindTableCommandIsEnabled
        {
            get => _findTableCommandIsEnabled;
            set => SetProperty(ref _findTableCommandIsEnabled, value);
        }

        #endregion Properties

        #region Commands

        public RelayCommand<string> FindTableCommand { get; }

        #endregion Commands

        #region Commands implementations

        private void _FindTableCommand(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                return;
            }

            FindTableCommandIsEnabled = false;

            Task task = null;

            _tablePropertiesMap.TryGetValue(param, out Dictionary<string, string> map);
            var entities = _databaseTableService.GetTableEntities(param);
            if (entities != null)
            {
                ColumnCollection.Clear();
                DataTable.Rows.Clear();
                DataTable.Columns.Clear();
                DataTable.Clear();

                task = Task.Run(() =>
               {
                   App.Current.Dispatcher.Invoke(() =>
                   {
                       _dataGridService.FillDataTable(DataTable, ColumnCollection, entities, map);
                   });
               });
            }
        }

        #endregion Commands implementations

        #region Other

        private void LoadTablePropertiesMap()
        {
            Dictionary<string, string> userPropertiesMap = new Dictionary<string, string>();
            userPropertiesMap.Add("ID", "№п/п");
            userPropertiesMap.Add("Login", "Логин");
            userPropertiesMap.Add("PasswordHash", "Хэш пароля");

            Dictionary<string, string> carDrivePropertiesMap = new Dictionary<string, string>();
            carDrivePropertiesMap.Add("ID", "№п/п");
            carDrivePropertiesMap.Add("Name", "Название");

            _tablePropertiesMap.Add("User", userPropertiesMap);
            _tablePropertiesMap.Add("CarDrive", carDrivePropertiesMap);
        }

        #endregion Other
    }
}