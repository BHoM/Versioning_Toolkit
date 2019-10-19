using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Upgrader.v30
{
    public partial class Converter 
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public IObject ToNew(oM.Architecture.Elements.Level level)
        {
            if (level == null)
                return null;

            return new oM.Geometry.SettingOut.Level
            {
                Elevation = level.Elevation
            };
        }

        /***************************************************/

        public IObject ToOld(oM.Geometry.SettingOut.Level level)
        {
            if (level == null)
                return null;

            return new oM.Architecture.Elements.Level
            {
                Elevation = level.Elevation
            };
        }

        /***************************************************/
    }
}
