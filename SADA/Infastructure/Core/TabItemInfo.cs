using System;

namespace SADA.Infastructure.Core
{
    public class TabItemInfo
    {
        public TabItemInfo(string name, TabObservableObject viewModel)
        {
            Name = name;
            ViewModel = viewModel;
            GUID = Guid.NewGuid();
        }

        public string Name { get; set; }
        public Guid GUID { get; }
        public TabObservableObject ViewModel { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}