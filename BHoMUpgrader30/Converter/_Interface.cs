using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Upgrader.v30
{
    public partial class Converter : Base.IConverter
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public object IToNew(object item)
        {
            if (item == null)
                return null;
            else
                return ToNew(item as dynamic);
        }

        /***************************************************/

        public object IToOld(object item)
        {
            if (item == null)
                return null;
            else
                return ToOld(item as dynamic);
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        public object ToNew(object item)
        {
            return null;
        }

        /***************************************************/

        public object ToOld(object item)
        {
            return null;
        }

        /***************************************************/
    }
}
