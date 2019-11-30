using System;
using System.Collections.Generic;
using System.Text;

namespace MyFiler.Domain.ValueObjects
{
    public class Comment : ValueObject<Comment>
    {
        public Comment(string value)
        {
            Value = value;
        }

        protected override bool EqualsCore(Comment other)
        {
            return Value == other.Value;
        }
        public string Value;

        public string DisplayValue
        {
            get
            {
                return Value.ToString();
            }
        }

        public string DisplayValueWithoutNewline
        {
            get
            {
                return Value.Replace(Environment.NewLine, "");
            }
        }
    }
}
