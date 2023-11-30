using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.Core
{
    /// <summary>
    /// Представляет вкладку с загрузкой данных
    /// </summary>
    public abstract class TabObservableObjectWithLoading: TabObservableObject
    {

        #region Fields
        private bool _isLoading = false;
        private bool _isLoaded = false;
        private AsyncRelayCommand _loadedCommand;

        #endregion

        #region Properties
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        protected abstract void LoadedInner();

        #endregion
        
        #region Commands
        public AsyncRelayCommand LoadedCommand
        {
            get
            {
                if(_loadedCommand == null)
                {
                    _loadedCommand = new AsyncRelayCommand(() => Task.Run(_LoadedCommand));
                }
                return _loadedCommand;
            }
            private set
            {
                _loadedCommand = value;
            }
        }
        #endregion

        #region Command implementation

        protected void _LoadedCommand()
        {
            if (_isLoaded)
            {
                return;
            }
            IsLoading = true;

            LoadedInner();

            IsLoading = false;
            _isLoaded = true;
        }

        #endregion

        #region Other
        #endregion

    }
}
