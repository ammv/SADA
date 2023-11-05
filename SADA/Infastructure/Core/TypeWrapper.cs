using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.Core
{
    class TypeWrapper<TBase>
    {

        public Type TypeBase { get => typeof(TBase); }

        public Type TypeDerived { get; private set; }

        private TypeWrapper() {}

        public static TypeWrapper<TBase> Make<TDerived>() where TDerived : TBase
        {
            return new TypeWrapper<TBase>
            {
                TypeDerived = typeof(TDerived)
            };
        }
    }
}
