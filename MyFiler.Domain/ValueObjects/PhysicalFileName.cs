using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFiler.Domain.ValueObjects
{
    public class PhysicalFileName : ValueObject<PhysicalFileName>
    {
        public PhysicalFileName(Guid value)
        {
            Value = value;
        }
        public Guid Value;
        protected override bool EqualsCore(PhysicalFileName other)
        {
            return Value == other.Value;
        }

        public string DisplayValue
        {
            get
            {
                return Value.ToString();
            }
        }
    }
}
