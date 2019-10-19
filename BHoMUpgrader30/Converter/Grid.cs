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

        public IObject ToNew(oM.Architecture.Elements.Grid grid)
        {
            if (grid == null)
                return null;

            return new oM.Geometry.SettingOut.Grid
            {
                Curve = grid.Curve
            };
        }

        /***************************************************/

        public IObject ToOld(oM.Geometry.SettingOut.Grid grid)
        {
            if (grid == null)
                return null;

            return new oM.Architecture.Elements.Grid
            {
                Curve = grid.Curve
            };
        }

        /***************************************************/
    }
}
