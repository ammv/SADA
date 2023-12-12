using CommunityToolkit.Mvvm.Input;
using SADA.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SADA.Infastructure.Core
{
    public class NavigationGroup
    {
        public NavigationGroup(List<NavigationItem> navigationItems, string header)
        {
            NavigationItems = navigationItems;
            Header = header;
        }

        public NavigationGroup(string header)
        {
            NavigationItems = new List<NavigationItem>();
            Header = header;
        }

        public List<NavigationItem> NavigationItems { get; set; }
        public string Header { get; set; }

        public NavigationGroup Add(ICommand command, string name)
        {
            NavigationItems.Add(new NavigationItem(command, name));
            return this;
        }

        public NavigationGroup Add(Action execute, string name)
        {
            NavigationItems.Add(new NavigationItem(new RelayCommand(execute), name));
            return this;
        }
    }
}
