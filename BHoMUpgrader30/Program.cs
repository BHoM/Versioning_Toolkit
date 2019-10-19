using System;
using System.IO;
using System.IO.Pipes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Architecture.Elements;

namespace BH.Upgrader.v30
{
    static class Program
    {
        /***************************************************/
        /**** Entry Methods                             ****/
        /***************************************************/

        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            Base.Upgrader upgrader = new Base.Upgrader();
            upgrader.ProcessingLoop(args[0], new Converter());
        }

        /***************************************************/
    }
}
