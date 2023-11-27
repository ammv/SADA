using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SADA.Infastructure.Validators
{
    class NumberValidationRule : ValidationRule
    {
        private Type _targetType = typeof(int);
        public virtual Type[] AllowNumberTypes { get; } = { typeof(int), typeof(uint), typeof(float), typeof(decimal), typeof(byte), typeof(short), typeof(long), typeof(double) };
        public Type TargetType
        {
            get => _targetType;
            set
            {
                if(AllowNumberTypes.Contains(value))
                {
                    _targetType = value;
                }
                else
                {
                    throw new ArgumentException($"Неподдерживаемый тип {value}");
                }
            }
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var validationResult = new ValidationResult(true, null);

            if(_targetType == null) { }
            else if(!(value is string s) || string.IsNullOrEmpty(s))
            {
                validationResult = new ValidationResult(false, "Пустое значение недопустимо");
            }
            else
            {
                try
                {
                    TypeDescriptor.GetConverter(_targetType).ConvertFromInvariantString(s);
                }
                catch(Exception)
                {
                    validationResult = new ValidationResult(false, $"Неккоректное значение");

                }

                
            }

            return validationResult;
        }
    }
}
