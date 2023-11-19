﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Windows.Input;

namespace SADA.Infastructure.Core
{
    public abstract class TabObservableObject : ObservableObject, ITab
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ICommand CloseCommand { get; protected set; }

        protected void _RaiseCloseEvent()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CloseRequested;

        public override string ToString()
        {
            return _name;
        }
    }
}