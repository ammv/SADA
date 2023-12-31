﻿using SADA.Infastructure.Core;
using SADA.ViewModel.Utils;

namespace SADA.Infastructure.ViewModelLocators.Childrens
{
    public class UtilsViewModelLocator : ViewModelLocatorBase
    {
        public WindowTopButtonsViewModel WindowTopButtons
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockWindowTopButtonsViewModel();
                }
                else
                {
                    return App.Current.GetService<WindowTopButtonsViewModel>();
                }
            }
        }
    }
}