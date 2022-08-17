using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Common
{
    public static class CommonHelper
    {
        public static string ToStringOrEmpty(this object value)
        {
            if (value == null) return string.Empty;
            return value.ToString();
        }
    }
}
