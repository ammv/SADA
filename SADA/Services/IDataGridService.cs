using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;

namespace SADA.Services
{
    interface IDataGridService
    {
        void FillDataTable(DataTable dataTable, ICollection<DataGridColumn> columns, List<object> entities, Dictionary<string, string> propertyMap);
    }
}
