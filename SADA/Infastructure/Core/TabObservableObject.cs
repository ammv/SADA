using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace SADA.Infastructure.Core
{
    /// <summary>
    /// Представляет простую вкладку с возможностью закрытия
    /// </summary>
    public abstract class TabObservableObject : ObservableObject, ITab
    {
        private string _name;
        private SolidColorBrush _brush = new SolidColorBrush();
        private RelayCommand<FrameworkElement> _changeBottomBrushCommand;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public object ViewModel
        {
            get => this;
        }

        public ICommand CloseCommand { get; protected set; }
        public SolidColorBrush BottomBrush 
        { 
            get => _brush;
            set => SetProperty(ref _brush, value);
        }

        public RelayCommand<FrameworkElement> ChangeBottomBrushCommand
        {
            get
            {
                return _changeBottomBrushCommand = _changeBottomBrushCommand ??
                    new RelayCommand<FrameworkElement>(_ChangeBottomBrushCommand);
            }
            set
            {
                _changeBottomBrushCommand = value;
            }
        }

        private void _ChangeBottomBrushCommand(FrameworkElement obj)
        {
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("BottomBrush");
            binding.Mode = BindingMode.OneWayToSource;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            

            var picker = SingleOpenHelper.CreateControl<ColorPicker>();
            var window = new PopupWindow
            {
                PopupElement = picker,
                
                
            };
            picker.Confirmed += delegate { window.Close(); };
            //picker.SelectedColorChanged += Picker_SelectedColorChanged;у мен
            picker.Canceled += delegate { window.Close(); };
            window.MouseLeftButtonDown += (s, e) => window.DragMove();
            window.Show(obj, false);

            BindingOperations.SetBinding(picker, ColorPicker.SelectedBrushProperty, binding);
        }

        private void Picker_SelectedColorChanged(object sender, HandyControl.Data.FunctionEventArgs<Color> e)
        {
            BottomBrush = new SolidColorBrush(e.Info);
        }

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