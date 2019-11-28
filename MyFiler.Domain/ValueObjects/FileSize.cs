using System;
using System.Collections.Generic;
using System.Text;

namespace MyFiler.Domain.ValueObjects
{
    public class FileSize : ValueObject<FileSize>
    {
        public FileSize(UInt64 value)
        {
            Value = value;
        }
        public UInt64 Value;
        protected override bool EqualsCore(FileSize other)
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

        readonly string[] suffixes =
        {
            "Bytes",
            "KB",
            "MB",
            "GB",
            "TB",
            "PB",
            "EB",
            "ZB",
            "YB",
        };

        public string DisplayValueWithUnit
        {
            get
            {
                // see (logic from):
                // https://stackoverflow.com/a/37111878
                int suffixIndex = 0;
                var size = (decimal)Value;
                while (Math.Round(size / 1024) >= 1)
                {
                    size /= 1024;
                    suffixIndex++;
                }
                return string.Format("{0:n2} {1}", size, suffixes[suffixIndex]);
            }
        }
    }
}
