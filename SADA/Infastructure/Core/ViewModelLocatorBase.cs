using System.ComponentModel;
using System.Windows;

namespace SADA.Infastructure.Core
{
    internal abstract class ViewModelLocatorBase
    {
        private DependencyObject dummy = new DependencyObject();

        protected bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(dummy);
        }
    }
}