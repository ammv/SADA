using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SADA.Infastructure.Core
{
    public class NavigationItem
    {
        public NavigationItem(ICommand command, string text)
        {
            Command = command;
            Text = text;
        }

        public ICommand Command { get; set; }
        public string Text { get; set; }
    }
}
