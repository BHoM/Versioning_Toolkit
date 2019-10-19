using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Versioning
{
    public class Parent : BHoMObject
    {
        public NewVersion Child { get; set; } = null;

        public double Number { get; set; } = 5;
    }
}