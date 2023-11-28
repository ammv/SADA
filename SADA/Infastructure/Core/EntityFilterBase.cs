using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SADA.Infastructure.Core
{
    public abstract class EntityFilterBase<T> : INotifyPropertyChanged
        where T : class, new()
    {
        protected readonly Expression<Func<T, bool>> defaultExpression = x => true;
        private readonly IQueryable<T> _defaultQuery = Enumerable.Empty<T>().AsQueryable<T>();
        private PropertyInfo[] _propertyInfos = null;

        public abstract Expression<Func<T, bool>> MakeFilter();

        public virtual IQueryable<T> Query => _defaultQuery.Where(MakeFilter());

        public event PropertyChangedEventHandler PropertyChanged;

        public void FilterFieldsClear()
        {
            _propertyInfos = _propertyInfos ?? this.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite).ToArray();

            foreach (var propertyInfo in _propertyInfos)
            {
                propertyInfo.SetValue(this, null);
                PropertyChanged(this, new PropertyChangedEventArgs(propertyInfo.Name));
            }
        }
    }
}