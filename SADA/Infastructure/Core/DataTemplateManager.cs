﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace SADA.Infastructure.Core
{
    class DataTemplateManager
    {
        private static string _projectName = Assembly.GetEntryAssembly().GetName().Name;
        private static Assembly _executingAssebly = Assembly.GetExecutingAssembly();
        public void RegisterDataTemplate<TView, TViewModel>()
        {
            var dataTemplate = _CreateDataTemplate(typeof(TView), typeof(TViewModel));
            Application.Current.Resources.Add(dataTemplate.DataTemplateKey, dataTemplate);
        }

        public void RegisterDataTemplateAuto()
        {
            var viewModels = _executingAssebly.GetTypes()
                .Where(
                    type => type.Namespace != null &&
                    type.Namespace.StartsWith($"{_projectName}.ViewModel") &&
                    type.Name.EndsWith("ViewModel") &&
                    !type.Name.StartsWith("Mock") &&
                    !type.Name.StartsWith("<")).ToList();

            var views = _executingAssebly.GetTypes()
                .Where(
                type => type.Namespace != null &&
                type.Namespace.StartsWith($"{_projectName}.View") &&
                type.Name.EndsWith("View") &&
                !type.Name.StartsWith("<")
                ).ToList();
            foreach (var viewModelType in viewModels)
            {
                var viewName = viewModelType.Name.Replace("ViewModel", "View");
                var viewType = views.FirstOrDefault(type => type.Name == viewName);
                if (viewType != null)
                {
                    var dataTemplate = _CreateDataTemplate(viewType, viewModelType);
                    Application.Current.Resources.Add(dataTemplate.DataTemplateKey, dataTemplate);
                }
            }
        }

        private DataTemplate _CreateDataTemplate(Type viewType, Type viewModelType)
        {
            const string xamlTemplate = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>";
            var xaml = String.Format(xamlTemplate, viewModelType.Name, viewType.Name, viewModelType.Namespace, viewType.Namespace);

            var context = new ParserContext();

            context.XamlTypeMapper = new XamlTypeMapper(new string[0]);
            context.XamlTypeMapper.AddMappingProcessingInstruction("vm", viewModelType.Namespace, viewModelType.Assembly.FullName);
            context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace, viewType.Assembly.FullName);

            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("vm", "vm");
            context.XmlnsDictionary.Add("v", "v");

            var template = (DataTemplate)XamlReader.Parse(xaml, context);
            return template;
        }
    }
}
