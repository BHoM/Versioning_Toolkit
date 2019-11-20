using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BH.Upgrader.Base
{
    public interface IConverter
    {
        /***************************************************/
        /**** Public Properties                         ****/
        /***************************************************/

        Dictionary<string, string> ToNewType { get; set; }

        Dictionary<string, string> ToOldType { get; set; }

        Dictionary<string, MethodBase> ToNewMethod { get; set; }

        Dictionary<string, MethodBase> ToOldMethod { get; set; }


        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        object IToNew(object item);

        object IToOld(object item);

        object IToNew(Dictionary<string, object> item, object typedObject);

        /***************************************************/
    }
}
