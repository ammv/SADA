using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyInfo.Name));
            }
        }

        public bool SetProperty<TProperty>(ref TProperty field, TProperty newValue, [CallerMemberName]string propertyName = null)
        {
            if (Equals(field, newValue))
                return false;
            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}