using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Helpers
{
    public static class FullNameToInitialsHelper
    {
        public static string MakeInitials(string firstName, string middleName, string lastName = null)
        {
            lastName = string.IsNullOrEmpty(lastName) ? string.Empty : $"{lastName[0]}.";
            return $"{middleName} {firstName[0]}. {lastName}";
        }
    }
}
