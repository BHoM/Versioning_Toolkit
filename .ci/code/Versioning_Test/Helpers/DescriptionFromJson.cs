using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Test.Versioning
{
    public static partial class Helpers
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        public static string DescriptionFromJson(string json)
        {
            string[] split = json.Split(new char[] { '"' });
            if (split.Length < 4)
                return "";

            string type = split[3];
            if (type == "System.Reflection.MethodBase")
            {
                if (split.Length < 20)
                    return "";

                string methodName = split[19];
                string declaringType = split[14].Split(new char[] { ',' }).First();
                return declaringType + "." + methodName;
            }
            else
                return type;
        }

        /*************************************/
    }
}
