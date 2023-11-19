using System;

namespace SADA.Infastructure.Core
{
    internal class TypeWrapper<TBase>
    {
        public Type TypeBase { get => typeof(TBase); }

        public Type TypeDerived { get; private set; }

        private TypeWrapper()
        { }

        public static TypeWrapper<TBase> Make<TDerived>() where TDerived : TBase
        {
            return new TypeWrapper<TBase>
            {
                TypeDerived = typeof(TDerived)
            };
        }
    }
}