using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.Infastructure.Core
{
    class ViewModelLocator
    {
        public static DependencyProperty AutoWireViewModelProperty = DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool),
        typeof(ViewModelLocator), new PropertyMetadata(false, AutoWireViewModelChanged));

        public static bool GetAutoWireViewModel(UIElement element)
        {
            return (bool)element.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(UIElement element, bool value)
        {
            element.SetValue(AutoWireViewModelProperty, value);
        }

        private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                Bind(d);
        }

        private static Type FindViewModel(Type viewType)
        {
            string viewName = viewType.FullName;

            if (viewType.FullName.EndsWith("Page"))
            {
                viewName = viewType.FullName
                    .Replace("Page", string.Empty);
            }


            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName.Replace("View", "ViewModel"), viewAssemblyName);

            return Type.GetType(viewModelName);
        }

        private static void Bind(DependencyObject view)
        {
            if (view is FrameworkElement frameworkElement)
            {
                var viewModelType = FindViewModel(frameworkElement.GetType());
                //frameworkElement.DataContext = Activator.CreateInstance(viewModelType);
                frameworkElement.DataContext = ActivatorUtilities.GetServiceOrCreateInstance(App.Current.Services, viewModelType);
            }
        }
    }
}
