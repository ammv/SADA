using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.Core.Enums
{
    public enum ComparisonOperator
    {
        [Description("")]
        NOT_SET,
        [Description("=")]
        Equals,
        [Description("!=")]
        NotEquals,
        [Description(">")]
        GreaterThen,
        [Description("<")]
        LowerThen,
        [Description(">=")]
        GreaterThenOrEquals,
        [Description("<=")]
        LowerThenOrEquals
    }
}
