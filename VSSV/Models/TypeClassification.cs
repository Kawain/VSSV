using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSSV.Models
{
    public class TypeClassification
    {
        //詳細不明
        public static Type Judge(string str)
        {
            str = str.ToLower();

            if (str.Contains("int") || str == "numeric" || str == "boolean")
            {
                return typeof(int);
            }
            else if (str == "text" || str.Contains("char") || str == "clob" || str == "blob")
            {
                return typeof(string);
            }
            else if (str == "real" || str.Contains("double") || str == "float" || str.Contains("decimal"))
            {
                return typeof(double);
            }

            return typeof(string);
        }
    }
}
