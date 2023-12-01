using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.Core
{
    public class CollectionWithSelection<T>: INotifyPropertyChanged
        where T: class
    {
        private T _selected;
        private ICollection<T> _collection;
        public T Selected 
        { 
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public ICollection<T> Collection
        {
            get => _collection;
            set => SetProperty(ref _collection, value);
        }

        public CollectionWithSelection(ICollection<T> collection = null, T selected = null)
        {
            _collection = collection;
            _selected = selected;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool SetProperty<TProperty>(ref TProperty field, TProperty newValue, [CallerMemberName]string propertyName = null)
        {
            if (Equals(field, newValue)) return false;
            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        public CollectionWithSelection<T> Clone()
        {
            return new CollectionWithSelection<T>(_collection, null);
        }
    }
}
