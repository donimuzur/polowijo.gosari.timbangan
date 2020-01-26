using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.core
{
    public static class FunctionHelper
    {
        public static bool IsValidDecimal(string str)
        {
            double i;
            return double.TryParse(str, out i) && i >= 1;
        }
    }
}
