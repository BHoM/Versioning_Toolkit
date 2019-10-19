using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Versioning
{
    public class NewVersion : BHoMObject
    {
        public double AplusB { get; set; } = 1;

        public double AminusB { get; set; } = 2;
    }
}