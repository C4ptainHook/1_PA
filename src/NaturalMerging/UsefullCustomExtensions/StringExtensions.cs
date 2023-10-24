using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalMerging.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string Join(char separator, ReadOnlySpan<string> NumericSpan )
        {
            StringBuilder sb = new StringBuilder();
            foreach ( string number in NumericSpan) 
            {
                sb.Append(number + separator);
            }
            return sb.ToString();
        }
    }
}
