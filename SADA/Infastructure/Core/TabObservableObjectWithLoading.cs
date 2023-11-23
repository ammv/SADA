using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.Core
{
    public abstract class TabObservableObjectWithLoading: TabObservableObject
    {

        #region Fields
        private bool _isLoading = false;
        private bool _isLoaded = false;
        private Action _loadedInnerAction = null;

        #endregion

        #region Constructor

        public TabObservableObjectWithLoading(Action loadedInnerAction = null)
        {
            LoadedCommand = new AsyncRelayCommand(() => Task.Run(_LoadedCommand));
            if(loadedInnerAction != null)
            {
                _loadedInnerAction = loadedInnerAction;
            }
        }

        #endregion

        #region Properties
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public Action LoadedInnerAction 
        { 
            get => _loadedInnerAction;
            set => _loadedInnerAction = value; 
        }

        #endregion

        #region Commands
        public AsyncRelayCommand LoadedCommand { get; }
        #endregion

        #region Command implementation

        protected virtual void _LoadedCommand()
        {
            if (_isLoaded)
            {
                return;
            }
            IsLoading = true;

            _loadedInnerAction?.Invoke();

            IsLoading = false;
            _isLoaded = true;
        }

        #endregion

        #region Other
        #endregion

    }
}
