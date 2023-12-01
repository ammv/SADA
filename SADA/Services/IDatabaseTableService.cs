using System.Collections.Generic;

namespace SADA.Services
{
    public interface IDatabaseTableService
    {
        bool ContainsTable(string tableName);

        List<object> GetTableEntities(string tableName);

        List<T> GetTableEntities<T>() where T : class;
    }
}