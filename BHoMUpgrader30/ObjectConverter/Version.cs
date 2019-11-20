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

        public IObject ToNew(oM.Versioning.OldVersion version)
        {
            if (version == null)
                return null;

            return new oM.Versioning.NewVersion
            {
                AplusB = version.A + version.B,
                AminusB = version.A - version.B
            };
        }

        /***************************************************/

        public IObject ToOld(oM.Versioning.NewVersion version)
        {
            if (version == null)
                return null;

            return new oM.Versioning.OldVersion
            {
                A = (version.AplusB + version.AminusB) / 2,
                B = (version.AplusB - version.AminusB) / 2,
            };
        }

        /***************************************************/

        public IObject ToNew(Dictionary<string, object> version, oM.Versioning.NewVersion typedObject)
        {
            if (version == null)
                return null;

            double A = (double)version["A"];
            double B = (double)version["B"];

            return new oM.Versioning.NewVersion
            {
                AplusB = A + B,
                AminusB = A - B
            };
        }

        /***************************************************/
    }
}
