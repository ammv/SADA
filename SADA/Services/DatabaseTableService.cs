using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SADA.Services
{
    public class DatabaseTableService : IDatabaseTableService, IDisposable
    {
        private readonly Type[] _entitiesTypes;
        private readonly DbContext _ctx;

        public DatabaseTableService(DbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));

            _entitiesTypes = _ctx.GetType().Assembly.GetTypes();
        }

        public bool ContainsTable(string tableName)
        {
            return _GetTableTypeByName(tableName) != null;
        }

        private Type _GetTableTypeByName(string tableName)
        {
            return _entitiesTypes.FirstOrDefault(t => t.Name == tableName);
        }

        public List<object> GetTableEntities(string tableName)
        {
            Type entityType = _GetTableTypeByName(tableName);
            if (entityType == null)
            {
                return null;
            }

            return _ctx.Set(entityType).ToListAsync().GetAwaiter().GetResult();
        }

        public List<T> GetTableEntities<T>() where T : class
        {
            return _ctx.Set<T>().ToListAsync().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            if (_ctx != null)
            {
                _ctx.Dispose();
            }
        }
    }
}