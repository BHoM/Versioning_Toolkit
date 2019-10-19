using BH.oM.Base;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Versioning
{
    [Deprecated("3.0", "Replaced by oM.Versioning.NewVersion")]
    public class OldVersion : BHoMObject
    {
        public double A { get; set; } = 1;

        public double B { get; set; } = 2;
    }
}