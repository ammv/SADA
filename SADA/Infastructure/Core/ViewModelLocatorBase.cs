using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.Infastructure.Core
{
    abstract class ViewModelLocatorBase
    {
        private DependencyObject dummy = new DependencyObject();

        protected bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(dummy);
        }
    }
}
