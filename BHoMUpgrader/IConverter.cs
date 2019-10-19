using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Upgrader.Base
{
    public interface IConverter
    {
        object IToNew(object item);

        object IToOld(object item);
    }
}
