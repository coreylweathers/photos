using System;
using System.Collections.Generic;
using System.Text;

namespace Photos.Shared.Extensions
{
    public static class ExtensionMethods
    {
        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }
    }
}
