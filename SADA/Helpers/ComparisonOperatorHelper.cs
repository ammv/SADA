using SADA.Infastructure.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SADA.Helpers
{
    public static class ComparisonOperatorHelper
    {
        public static Dictionary<ComparisonOperator, string> ComparisonOperatorMap = new Dictionary<ComparisonOperator, string>
        {
            {ComparisonOperator.NOT_SET, string.Empty },
            {ComparisonOperator.Equals, "=" },
            {ComparisonOperator.GreaterThen, ">" },
            {ComparisonOperator.GreaterThenOrEquals, ">=" },
            {ComparisonOperator.LowerThen, "<" },
            {ComparisonOperator.LowerThenOrEquals, "<=" },
            {ComparisonOperator.NotEquals, "!=" },
        };

        public static Dictionary<string, ComparisonOperator> ComparisonOperatorReverseMap = ComparisonOperatorMap.ToDictionary(d => d.Value, d => d.Key);

        public static bool Compare(double right, double left, ComparisonOperator comparisonOperator)
        {
            switch(comparisonOperator)
            {
                case ComparisonOperator.NOT_SET:
                    return true;
                case ComparisonOperator.Equals:
                    return right == left;
                case ComparisonOperator.NotEquals:
                    return right != left;
                case ComparisonOperator.GreaterThen:
                    return right > left;
                case ComparisonOperator.GreaterThenOrEquals:
                    return right >= left;
                case ComparisonOperator.LowerThen:
                    return right < left;
                case ComparisonOperator.LowerThenOrEquals:
                    return right <= left;
                default:
                    throw new ArgumentOutOfRangeException(nameof(comparisonOperator));
            }
        }
    }
}