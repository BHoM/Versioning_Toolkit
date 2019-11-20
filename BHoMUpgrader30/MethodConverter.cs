using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BH.Upgrader.v30
{
    public partial class Converter
    {
        /***************************************************/
        /**** Public Properties                         ****/
        /***************************************************/

        public Dictionary<string, MethodBase> ToNewMethod { get; set; } = new Dictionary<string, MethodBase>
        {
            {
                "BH.UI.Components.DeleteCaller.Delete(BH.Adapter.BHoMAdapter, BH.oM.Data.Requests.FilterRequest, System.Collections.Generic.Dictionary<System.String, System.Object>, System.Boolean)",
                typeof(BH.UI.Components.RemoveCaller).GetMethod("Remove")
            },
            {
                "BH.UI.Components.MoveCaller.Move(BH.Adapter.BHoMAdapter, BH.Adapter.BHoMAdapter, BH.oM.Data.Requests.IRequest, System.Collections.Generic.Dictionary<System.String, System.Object>, System.Collections.Generic.Dictionary<System.String, System.Object>, System.Boolean)",
                typeof(BH.UI.Components.MoveCaller).GetMethod("Move")
            },
            {
                "BH.UI.Components.PullCaller.Pull(BH.Adapter.BHoMAdapter, BH.oM.Data.Requests.IRequest, System.Collections.Generic.Dictionary<System.String, System.Object>, System.Boolean)",
                typeof(BH.UI.Components.PullCaller).GetMethod("Pull")
            },
            {
                "BH.UI.Components.PushCaller.Push(BH.Adapter.BHoMAdapter, System.Collections.Generic.IEnumerable<BH.oM.Base.IObject>, System.String, System.Collections.Generic.Dictionary<System.String, System.Object>, System.Boolean)",
                typeof(BH.UI.Components.PushCaller).GetMethod("Push")
            }
        };

        /***************************************************/

        public Dictionary<string, MethodBase> ToOldMethod { get; set; } = new Dictionary<string, MethodBase>
        {

        };

        /***************************************************/
    }
}
