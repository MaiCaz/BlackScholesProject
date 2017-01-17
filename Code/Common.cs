using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackScholesModelisation
{
    public static class Common
    {
        public static double? ConvertToDouble(string value) {
            if (value == null || value == "") {
                return null;
            }
            else {
                try {
                    double outVal = double.Parse(value);
                    return outVal;
                }
                catch {
                    return null;
                }
            }
        }
    }
   }
