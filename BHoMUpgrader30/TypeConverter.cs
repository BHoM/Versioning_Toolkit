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
        /**** Public Properties                         ****/
        /***************************************************/

        public Dictionary<string, string> ToNewType { get; set; } = new Dictionary<string, string>
        {
            { "BH.oM.Deprecated", "BH.oM.Versioning" },
            { "BH.oM.OldStuff.OldVersion", "BH.oM.Versioning.NewVersion" },
            { "BH.oM.OldStuff.Parent", "BH.oM.Versioning.parent" }
        };

        /***************************************************/

        public Dictionary<string, string> ToOldType { get; set; } = new Dictionary<string, string>
        {
            { "BH.oM.Versioning", "BH.oM.Deprecated" },
            { "BH.oM.Versioning.NewVersion", "BH.oM.OldStuff.OldVersion" },
            { "BH.oM.Versioning.parent", "BH.oM.OldStuff.Parent" }
        };

        /***************************************************/
    }
}
