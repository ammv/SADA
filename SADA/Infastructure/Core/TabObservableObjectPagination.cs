﻿using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.Core
{
    public abstract class TabObservableObjectPagination<T>: TabObservableObjectWithLoading
        where T: class, new()
    {

        #region Fields

        protected ObservableCollection<T> _entities;
        protected int _dataCountPerPage = 20;
        protected int _maxPage = 0;
        protected T _selectedEntity = null;

        //protected abstract IQueryable<T> _defaultQuery { get; }
        //protected IQueryable<T> _currentQuery { get; }

        #endregion

        #region Constructor
        #endregion

        #region Properties
        public virtual ObservableCollection<T> Entities
        {
            get { return _entities; }
            set { SetProperty(ref _entities, value); }
        }

        public virtual int DataCountPerPage
        {
            get { return _dataCountPerPage; }
            set
            {
                if (SetProperty(ref _dataCountPerPage, value))
                {
                    MaxPage = _entities.Count() / _dataCountPerPage + 1;
                }
            }
        }

        public virtual int MaxPage
        {
            get { return _maxPage; }
            set { SetProperty(ref _maxPage, value); }
        }

        public virtual T SelectedEntity
        {
            get { return _selectedEntity; }
            set { SetProperty(ref _selectedEntity, value); }
        }
        #endregion

        #region Commands
        public AsyncRelayCommand<HandyControl.Data.FunctionEventArgs<int>> PageUpdateCommand
        {
            get
            {
                if(PageUpdateCommand == null)
                {
                    PageUpdateCommand = new AsyncRelayCommand<HandyControl.Data.FunctionEventArgs<int>>(_PageUpdateCommand);
                }
                return PageUpdateCommand;
            }
            protected set
            {
                PageUpdateCommand = value;
            }
        }

        #endregion

        #region Command implementation

        protected abstract Task _PageUpdateCommand(HandyControl.Data.FunctionEventArgs<int> e);

        #endregion

        #region Other
        #endregion

    }
}
