using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;

namespace SADA.Services
{
    class DataGridService : IDataGridService
    {
        public void FillDataTable(DataTable dataTable, ICollection<DataGridColumn> columns, List<object> entities, Dictionary<string, string> propertyMap)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException(nameof(dataTable));
            }
            else if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }
            else if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            List<PropertyInfo> propertiesInfo;

            {
                object entity = entities.First();
                propertiesInfo = entity.GetType().GetProperties().ToList();
                if (propertyMap != null)
                {
                    propertiesInfo = propertiesInfo.Where(t => propertyMap.ContainsKey(t.Name)).ToList();
                }
                else
                {
                    propertiesInfo = propertiesInfo.Where(t => !t.GetGetMethod().IsVirtual).ToList();
                }

            }

            foreach (var prop in propertiesInfo)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                DataColumn dataColumn = new DataColumn();
                dataColumn.ColumnName = prop.Name;

                Type underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);

                if (underlyingType != null)
                {
                    dataColumn.AllowDBNull = true;
                    dataColumn.DataType = underlyingType;
                }
                else
                {
                    dataColumn.DataType = prop.PropertyType;
                }

                dataTable.Columns.Add(dataColumn);

                if (propertyMap != null)
                {
                    column.Header = propertyMap[prop.Name];
                }
                else
                {
                    column.Header = prop.Name;
                }

                Binding binding = new Binding(prop.Name);
                binding.TargetNullValue = "-";
                column.Binding = binding;

                columns.Add(column);
            }

            foreach (var entity in entities)
            {
                var row = dataTable.NewRow();

                for (int i = 0; i < propertiesInfo.Count; i++)
                {
                    row[propertiesInfo[i].Name] = propertiesInfo[i].GetValue(entity) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

        }
    }
}
